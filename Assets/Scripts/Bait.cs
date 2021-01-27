using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bait : MonoBehaviour, IUpdate
{
    bool sale;
    bool returning;
    Vector3 myInitialPos;
    Vector3 truePos;
    Transform myParent;
    Rigidbody rb;

    Action ReachTargetPos;
    Action ReachInitialPos;

    [SerializeField] float potency = 5;

    private void Awake()
    {
        myParent = transform.parent;
        rb = GetComponent<Rigidbody>();
        truePos = transform.localPosition;
    }

    public void OnUpdate()
    {
        if (returning)
        {

            if (Vector3.Distance(myInitialPos, transform.position) <= 0.2f)
            {
                rb.velocity = Vector3.zero;
                rb.constraints = RigidbodyConstraints.FreezeAll;
                ReachInitialPos?.Invoke();
                transform.SetParent(myParent);
                transform.localPosition = truePos;
                returning = false;
                GameManager.instance.updateManager.DesuscribeToUpdate(this);
            }
        }
    }

    public void GoToPos(Vector3 targetPos, Action callback)
    {
        myInitialPos = transform.position;
        transform.parent = null;
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        sale = true;
        ReachTargetPos = callback;
        Vector3 dir = (targetPos - transform.position).normalized;
        rb.AddForce(dir * potency, ForceMode.Impulse);
    }

    public void ReturnBait(Action callback)
    {
        returning = true;
        ReachInitialPos = callback;
        Vector3 dir = (myInitialPos- transform.position).normalized;
        rb.AddForce(dir * potency, ForceMode.Impulse);
        GameManager.instance.updateManager.SuscribeToUpdate(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!sale) return;

        if(collision.gameObject.layer == 10)
        {
            sale = false;
            rb.velocity = Vector3.zero;
            ReachTargetPos?.Invoke();
        }
    }

}
