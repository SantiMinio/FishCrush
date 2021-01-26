using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelFish : Fish
{
    [SerializeField] float minSpeed = 13;
    [SerializeField] float maxSpeed = 18;
    [SerializeField] float slownessToReach = 10;
    float currentSpeed;

    [SerializeField, Range(0, 0.5f)] float barMaxPercentToGo = 0.2f;

    [SerializeField] float minTimeSlow = 1;
    [SerializeField] float maxTimeSlow = 2;
    float timer;
    float downMinimun;
    float upMinimun;
    float currentMaxTime;

    bool moving = true;
    bool inUp = false;
    float currentTargetPos;
    
    float dir = 1;

    public override void StartFishing(float _minPos, float _maxPos, float initialPos)
    {
        base.StartFishing(_minPos, _maxPos, initialPos);

        var percent = _maxPos * barMaxPercentToGo;
        downMinimun = _minPos + percent;
        upMinimun = maxPos - percent;
        currentTargetPos = Random.Range(upMinimun, maxPos);
        currentSpeed = Random.Range(minSpeed, maxSpeed);
    }

    public override void MoveInBattle()
    {
        currentPos += Time.deltaTime * currentSpeed * dir;

        if(currentPos >= maxPos)
        {
            currentPos = maxPos;
            dir *= -1;
        }
        else if (currentPos <= minPos)
        {
            currentPos = minPos;
            dir *= -1;
        }

        if (moving)
        {
            if (inUp)
            {
                if (currentPos <= currentTargetPos)
                {
                    currentSpeed /= slownessToReach;
                    currentMaxTime = Random.Range(minTimeSlow, maxTimeSlow);
                    moving = false;
                    inUp = false;
                }
            }
            else
            {
                if (currentPos >= currentTargetPos)
                {
                    currentSpeed /= slownessToReach;
                    currentMaxTime = Random.Range(minTimeSlow, maxTimeSlow);
                    moving = false;
                    inUp = true;
                }
            }
        }
        else
        {
            timer += Time.deltaTime;

            if (timer >= currentMaxTime)
            {
                timer = 0;
                moving = true;
                currentSpeed = Random.Range(minSpeed, maxSpeed);

                if (inUp) currentTargetPos = Random.Range(minPos, downMinimun);
                else currentTargetPos = Random.Range(upMinimun, maxPos);
            }
        }
    }
}
