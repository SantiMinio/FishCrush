using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public EventManager eventManager = new EventManager();

    public bool Fishing { get; private set; }

    [SerializeField] GameObject baitObject = null;
    [SerializeField] float baitRadious = 7;
    [SerializeField] float captureRadious = 1;
    bool baiting;
    List<Fish> fishOnBait = new List<Fish>();

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (baiting)
        {
            var overlap = Physics.OverlapSphere(baitObject.transform.position, baitRadious).
                Where(x=>x.GetComponent<Fish>()).
                Select(x => x.GetComponent<Fish>()).
                Where(x=> !fishOnBait.Contains(x));

            foreach (var item in overlap)
            {
                item.Move(baitObject.transform.position);

                if (Vector3.Distance(item.transform.position, baitObject.transform.position) < captureRadious) fishOnBait.Add(item);
                Debug.Log("atrapo a alguien?");
            }
        }
    }

    public void BaitFish(Fish _fish)
    {
        Fishing = true;
        UIManager.instance.ActiveFishBar(true);
        FishingManager.instance.StartFishing(_fish);
        eventManager.TriggerEvent(GameEvents.FishInBait);
    }

    public void FishingOver(bool fishCaptured, Fish fishType = null)
    {
        Fishing = false;
        UIManager.instance.ActiveFishBar(false);
        if (fishCaptured) UIManager.instance.AddFish();
        baitObject.SetActive(false);
        eventManager.TriggerEvent(GameEvents.FishingOver, fishCaptured);
    }

    public void DropBait(Vector3 pos, Vector3 startPos)
    {
        baitObject.SetActive(true);
        baitObject.transform.position = new Vector3(pos.x, baitObject.transform.position.y, pos.z);
        baiting = true;
    }

    public void GrabBait()
    {
        baiting = false;

        if(fishOnBait.Count > 0)
            BaitFish(NeareastFish());
        else
            baitObject.SetActive(false);

        fishOnBait.Clear();
    }

    Fish NeareastFish(int index = 0, Fish result = null)
    {
        if (index >= fishOnBait.Count)
            return result;

        if (index == 0) result = fishOnBait[index];
        else
        {
            float distanceOne = (result.transform.position - baitObject.transform.position).sqrMagnitude;
            float distanceTwo = (fishOnBait[index].transform.position - baitObject.transform.position).sqrMagnitude;

            if(distanceOne>distanceTwo) result = fishOnBait[index];
        }

        return NeareastFish(index + 1, result);
    }
}
