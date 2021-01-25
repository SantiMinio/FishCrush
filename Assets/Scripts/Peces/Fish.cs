using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fish : MonoBehaviour
{
    protected float minPos;
    protected float maxPos;
    public float currentPos;

    [SerializeField] float speedOutOfCombat = 2;

    public virtual void StartFishing(float _minPos, float _maxPos, float initialPos)
    {
        minPos = _minPos;
        maxPos = _maxPos;
        currentPos = initialPos;
    }

    public abstract void MoveInBattle();

    public void Move(Vector3 baitPos)
    {
         transform.forward = (baitPos-transform.position).normalized;
         transform.position += transform.forward * speedOutOfCombat * Time.deltaTime;
    }

    public virtual void ReturnToPool()
    {
        Destroy(this.gameObject);
    }
}
