using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Controller : MonoBehaviour
{
    Vector3 startPos;
    bool touchInTarget;
    bool fishing;
    bool fishingRoad;

    [SerializeField] float armPotencyMultiplier = 2;
    [SerializeField] float minRange = 1;
    [SerializeField] float maxRange = 6;

    private void Start()
    {
        GameManager.instance.eventManager.SubscribeToEvent(GameEvents.FishInBait, Baiting);
        GameManager.instance.eventManager.SubscribeToEvent(GameEvents.FishingOver, FishingOver);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up,out hit, 8000, 1 << 10))
            startPos = hit.point;
    }

    private void Update()
    {
        Inputs();
    }

    void Baiting()
    {
        fishing = true;
        fishingRoad = false;
    }

    void FishingOver()
    {
        fishing = false;
    }

    void Inputs()
    {
        if (fishing)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0)) FishingManager.instance.moveBar = true;
            else if (Input.GetKeyUp(KeyCode.Mouse0)) FishingManager.instance.moveBar = false;
        }
        else
        {
            if (!fishingRoad)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if(Physics.Raycast(ray, 8000))
                    {
                        touchInTarget = true;
                        UIManager.instance.ActivateArrow(true);
                    }
                }

                if (touchInTarget)
                {
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    Vector3 pos = Vector3.zero;
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 8000, 1<<10))
                        pos = hit.point;

                    Vector3 dir = startPos - pos;
                    dir = new Vector3(dir.x, 0, dir.z);
                    if(dir != Vector3.zero) transform.forward = dir.normalized;
                    Vector3 temp = dir * armPotencyMultiplier;
                    float trueMultiplier = Mathf.Clamp(temp.magnitude, minRange, maxRange);

                    UIManager.instance.UpdateArrowScale(trueMultiplier / 2);

                    if (Input.GetKeyUp(KeyCode.Mouse0))
                    {
                        touchInTarget = false;
                        fishingRoad = true;
                        Vector3 trueDir = transform.forward * trueMultiplier;
                        GameManager.instance.DropBait(transform.position + trueDir, transform.position);
                        UIManager.instance.ActivateArrow(false);
                    }
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    GameManager.instance.GrabBait();
                    fishingRoad = false;
                }
            }
        }
    }
}
