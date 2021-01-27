using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum NodePosition { Up = 0, Down = 1, Right = 2, Left = 3 }
public class FishSpawner : MonoBehaviour, IUpdate
{
    [SerializeField] NodesList myNodes = new NodesList();
    [SerializeField] FishSpawnProbability fishToSpawn = new FishSpawnProbability();

    Dictionary<Fish, Pool<Fish>> pools = new Dictionary<Fish, Pool<Fish>>();

    [SerializeField] int maxFishToSpawn = 8;

    [SerializeField] float minTimeToSpawn = 6;
    [SerializeField] float maxTimeToSpawn = 12;

    Dictionary<NodePosition, int> fishPerZone = new Dictionary<NodePosition, int>();

    int maxPerZone;

    private void Start()
    {
        maxPerZone = maxFishToSpawn / 4;
        for (int i = 0; i < 4; i++)
            fishPerZone.Add((NodePosition)i, 0);
        foreach (var item in fishToSpawn)
            pools.Add(item.Key, new Pool<Fish>(()=>FactoryMethod(item.Key), ActivateObject, Desactivate));
    }


    public void OnUpdate()
    {

    }

    void SpawnFish()
    {
        NodePosition pos = SelectZone();

        Node nodeToSpawn = myNodes[pos][Random.Range(0, myNodes[pos].Count)];

        Fish tempFish = pools[RoulletteWheel.Roullette(fishToSpawn)].GetObject();
    }

    NodePosition SelectZone()
    {
        Dictionary<NodePosition, int> temp = new Dictionary<NodePosition, int>();
        for (int i = 0; i < 4; i++)
        {
            temp.Add((NodePosition)i, 2 - fishPerZone[(NodePosition)i]);
        }

        return RoulletteWheel.Roullette(temp);
    }

    public void ReturnFish(Fish f)
    {
        f.ReturnToPool();
        pools[f].ReturnObject(f);
    }

    Fish FactoryMethod(Fish toSpawn)
    {
        return Instantiate(toSpawn, transform);
    }

    void ActivateObject(Fish f)
    {
        f.gameObject.SetActive(true);
        f.Initialize();
    }

    void Desactivate(Fish f)
    {
        f.gameObject.SetActive(false);
    }
}
