using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum NodePosition { Up = 0, Down = 1, Right = 2, Left = 3 }
public class FishSpawner : MonoBehaviour, IUpdate
{
    public NodesList myNodes = new NodesList();
    [SerializeField] FishSpawnProbability fishToSpawn = new FishSpawnProbability();

    Dictionary<Type, Pool<Fish>> pools = new Dictionary<Type, Pool<Fish>>();

    [SerializeField] int maxFishToSpawn = 8;
    [SerializeField] int initialFishAmmount = 1;

    [SerializeField] float minTimeToSpawn = 6;
    [SerializeField] float maxTimeToSpawn = 12;

    float timer;
    float currentTime;

    Dictionary<NodePosition, int> fishPerZone = new Dictionary<NodePosition, int>();

    int maxPerZone;
    int currentFishInScreen;

    private void Start()
    {
        maxPerZone = maxFishToSpawn / 4;
        for (int i = 0; i < 4; i++)
            fishPerZone.Add((NodePosition)i, 0);
        foreach (var item in fishToSpawn)
            pools.Add(item.Key.GetType(), new Pool<Fish>(()=>FactoryMethod(item.Key), ActivateObject, Desactivate));

        for (int i = 0; i < initialFishAmmount; i++) SpawnFish();
        GameManager.instance.updateManager.SuscribeToUpdate(this);
    }


    public void OnUpdate()
    {
        if (currentFishInScreen >= maxFishToSpawn) return;

        timer += Time.deltaTime;

        if (timer >= currentTime) SpawnFish();
    }

    void SpawnFish()
    {
        NodePosition pos = SelectZone();

        Node nodeToSpawn = myNodes[pos][UnityEngine.Random.Range(0, myNodes[pos].Count)];

        Fish tempFish = pools[RoulletteWheel.Roullette(fishToSpawn).GetType()].GetObject();

        tempFish.transform.position = nodeToSpawn.transform.position;

        tempFish.myNodeSpawn = nodeToSpawn;
        fishPerZone[pos] += 1;
        currentFishInScreen += 1;
        myNodes[pos].Remove(nodeToSpawn);

        timer = 0;
        currentTime = UnityEngine.Random.Range(minTimeToSpawn, maxTimeToSpawn);
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
        Node node = f.myNodeSpawn;
        fishPerZone[node.myPosition] -= 1;
        currentFishInScreen -= 1;
        myNodes[node.myPosition].Add(node);

        f.ReturnToPool();
        pools[f.GetType()].ReturnObject(f);
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
