using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour, IUpdate
{
    public static GameManager instance { get; private set; }

    public EventManager eventManager = new EventManager();
    public UpdateManager updateManager = new UpdateManager();

    public bool Fishing { get; private set; }

    [SerializeField] GameObject baitObject = null;
    [SerializeField] float baitRadious = 7;
    [SerializeField] float captureRadious = 1;
    bool baiting;
    List<Fish> fishOnBait = new List<Fish>();

    private void Awake()
    {
        instance = this;
        updateManager.SuscribeToUpdate(this);
    }

    private void Update()
    {
        updateManager.OnUpdate();
    }

    public void OnUpdate()
    {
        if (baiting)
        {
            var overlap = Physics.OverlapSphere(baitObject.transform.position, baitRadious).
                Where(x => x.GetComponent<Fish>()).
                Select(x => x.GetComponent<Fish>()).
                Where(x => !fishOnBait.Contains(x)).
                Where(x => x.InFishBait(baitObject.transform.position));

            foreach (var item in overlap)
            {
                item.Move(baitObject.transform.position);

                if (Vector3.Distance(item.transform.position, baitObject.transform.position) < captureRadious) { fishOnBait.Add(item); item.FishCaptured(true); }
            }
        }
    }

    public void BaitFish(Fish _fish)
    {
        updateManager.stopUpdate = true;
        Fishing = true;
        UIManager.instance.ActiveFishBar(true);
        FishingManager.instance.StartFishing(_fish);
        eventManager.TriggerEvent(GameEvents.FishInBait);
    }

    public void FishingOver(bool fishCaptured, Fish fishType = null)
    {
        updateManager.stopUpdate = false;
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

    public bool GrabBait()
    {
        baiting = false;
        bool result;

        if(fishOnBait.Count > 0)
        {
            BaitFish(NeareastFish());
            result = true;
        }
        else
        {
            baitObject.SetActive(false);
            result = false;
        }

        for (int i = 0; i < fishOnBait.Count; i++)
        {
            fishOnBait[i].FishCaptured(false);
        }
        fishOnBait.Clear();
        return result;
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
