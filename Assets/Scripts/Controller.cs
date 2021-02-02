using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class Controller : MonoBehaviour
{
    Vector3 startPos;
    Vector3 trueDir;
    bool touchInTarget;
    bool fishing;
    bool fishingRoad;
    bool inAnim;

    [SerializeField] Animator anim = null;

    [SerializeField] float armPotencyMultiplier = 2;
    [SerializeField] float minRange = 1;
    [SerializeField] float maxRange = 6;

    Func<bool> TouchDown;
    Func<bool> TouchUp;
    Func<bool> Touching;
    Func<Vector3> TouchPos;

    private void Start()
    {
        GameManager.instance.eventManager.SubscribeToEvent(GameEvents.FishInBait, Baiting);
        GameManager.instance.eventManager.SubscribeToEvent(GameEvents.FishingOver, FishingOver);
        var animEvent = anim.GetComponent<AnimEvent>();
        animEvent.Add_Callback("CastEvent", CastEvent);
        animEvent.Add_Callback("PullEvent", PullEvent);
        animEvent.Add_Callback("AnimOver", CanControl);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up,out hit, 8000, 1 << 10))
            startPos = hit.point;

        if(Application.platform == RuntimePlatform.Android)
        {
            TouchDown = ()=> Input.GetTouch(0).phase == TouchPhase.Began;
            TouchUp = () => Input.GetTouch(0).phase == TouchPhase.Ended;
            Touching = () => Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved;
            TouchPos = () => Input.GetTouch(0).position;
        }
        else
        {
            TouchDown = () => Input.GetKeyDown(KeyCode.Mouse0);
            TouchUp = () => Input.GetKeyUp(KeyCode.Mouse0);
            Touching = () => Input.GetKey(KeyCode.Mouse0);
            TouchPos = () => Input.mousePosition;
        }
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

    void FishingOver(params object[] param)
    {
        fishing = false;

        if ((bool)param[0]) anim.SetBool("Captured", true);
        else anim.SetBool("Captured", false);

        inAnim = true;

        anim.SetBool("Pull", false);
    }

    void Inputs()
    {
        if (inAnim) return;
        if (fishing)
        {
            if (TouchDown()) FishingManager.instance.moveBar = true;
            else if (TouchUp()) FishingManager.instance.moveBar = false;
        }
        else
        {
            if (!fishingRoad)
            {
                if (TouchDown())
                {
                    var ray = Camera.main.ScreenPointToRay(TouchPos());

                    if(Physics.Raycast(ray, 8000))
                    {
                        touchInTarget = true;
                        UIManager.instance.ActivateArrow(true);
                        anim.SetBool("Casting", true);
                    }
                }

                if (touchInTarget)
                {
                    var ray = Camera.main.ScreenPointToRay(TouchPos());
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

                    if (TouchUp())
                    {
                        anim.SetBool("Casting", false);
                        touchInTarget = false;
                        fishingRoad = true;
                        trueDir = transform.forward * trueMultiplier;
                        UIManager.instance.ActivateArrow(false);
                    }
                }
            }
            else
            {
                if (TouchDown())
                {
                    anim.SetBool("Pull", true);
                    inAnim = true;
                }
            }
        }
    }

    void CastEvent()
    {
        GameManager.instance.DropBait(transform.position + trueDir, transform.position);
    }

    void PullEvent()
    {
        if (!GameManager.instance.GrabBait())
        {
            anim.SetBool("Pull", false);
            anim.SetBool("Captured", false);
        }
        else
            inAnim = false;
        fishingRoad = false;
    }

    void CanControl() => inAnim = false;
}
