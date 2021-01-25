using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFish : Fish
{
    [SerializeField] float speed = 3;

    bool dir = true;

    public override void MoveInBattle()
    {
        if (dir)
        {
            currentPos += Time.deltaTime * speed;
            if (currentPos > maxPos)
            {
                dir = false;
                currentPos = maxPos;
            }
        }
        else
        {
            currentPos -= Time.deltaTime * speed;
            if (currentPos < minPos)
            {
                dir = true;
                currentPos = minPos;
            }
        }
    }
}
