using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bait : MonoBehaviour, IUpdate
{
    bool sale;
    bool returning;
    Vector3 myInitialPos;
    Vector3 targetPos;
    Vector3 trueInitialPos;
    Transform myParent;

    Action ReachTargetPos;
    Action ReachInitialPos;

    [SerializeField] float minSpeed = 0.5f;
    [SerializeField] float maxSpeed = 1;
    [SerializeField] float acceleration = 2;
    float currentSpeed = 0;
    float currentLerping = 0;

    private void Awake()
    {
        myParent = transform.parent;
    }

    public void OnUpdate()
    {
        if (returning)
        {
            currentLerping += currentSpeed * Time.deltaTime;
            if (currentSpeed < maxSpeed) currentSpeed = Mathf.Clamp(currentSpeed + Time.deltaTime * acceleration, minSpeed, maxSpeed);
            transform.position = Vector3.Lerp(targetPos, myParent.position, currentLerping);
            transform.rotation = Quaternion.Lerp(transform.rotation, myParent.rotation, currentLerping);

            if (currentLerping >= 1)
            {
                ReachInitialPos?.Invoke();
                transform.SetParent(myParent);
                returning = false;
                GameManager.instance.updateManager.DesuscribeToUpdate(this);
            }
        }
        else if (sale)
        {
            currentLerping += currentSpeed * Time.deltaTime;
            if (currentSpeed < maxSpeed) currentSpeed = Mathf.Clamp(currentSpeed + Time.deltaTime * acceleration,minSpeed,maxSpeed);
            transform.position = Vector3.Lerp(myInitialPos, targetPos, currentLerping);

            if (currentLerping >= 1)
            {
                sale = false;
                ReachTargetPos?.Invoke();
                GameManager.instance.updateManager.DesuscribeToUpdate(this);
            }
        }
    }

    public void GoToPos(Vector3 _targetPos, Action callback)
    {
        trueInitialPos = transform.localPosition;
        myInitialPos = transform.position;
        targetPos = _targetPos;
        transform.parent = null;
        sale = true;
        ReachTargetPos = callback;
        currentSpeed = minSpeed;
        currentLerping = 0;
        GameManager.instance.updateManager.SuscribeToUpdate(this);
    }

    public void ReturnBait(Action callback)
    {
        returning = true;
        ReachInitialPos = callback;
        currentSpeed = minSpeed;
        currentLerping = 0;
        GameManager.instance.updateManager.SuscribeToUpdate(this);
    }

}
