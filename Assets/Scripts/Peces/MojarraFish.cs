using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MojarraFish : Fish
{
    [SerializeField] float minSpeed = 1;
    [SerializeField] float maxSpeed = 10;
    float currentSpeed;

    [SerializeField, Range(0,1)] float barMaxPercentToGo = 0.5f;

    [SerializeField] float minTimeMoving = 0.5f;
    [SerializeField] float maxTimeMoving = 3;
    [SerializeField] float minTimeIdle = 2;
    [SerializeField] float maxTimeIdle = 5;
    float currentMaxTime;
    float timer;

    bool moving = true;

    float dir = 1;

    public override void StartFishing(float _minPos, float _maxPos, float initialPos)
    {
        base.StartFishing(_minPos, _maxPos, initialPos);
        maxPos *= barMaxPercentToGo;
        currentMaxTime = Random.Range(minTimeMoving, maxTimeMoving);
        currentSpeed = Random.Range(minSpeed, maxSpeed);

    }

    public override void MoveInBattle()
    {
        currentPos += Time.deltaTime * currentSpeed * dir;

        if (currentPos > maxPos) currentPos = maxPos;
        else if (currentPos < minPos) currentPos = minPos;

        timer += Time.deltaTime;

        if (timer >= currentMaxTime)
        {
            timer = 0;
            moving = !moving;

            if (moving)
            {
                currentSpeed = 0;
                currentMaxTime = Random.Range(minTimeIdle, maxTimeIdle);
            }
            else
            {
                currentSpeed = Random.Range(minSpeed, maxSpeed);
                currentMaxTime = Random.Range(minTimeMoving, maxTimeMoving);
                if (currentPos == minPos || currentPos == maxPos) dir *= -1;
                else
                {
                    int temp = Random.Range(0, 2);
                    if (temp == 0) dir = -1;
                    else dir = 1;
                }
            }
        }
    }
}
