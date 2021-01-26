using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementModule : MonoBehaviour
{
    protected float speed;
    protected float rotationSpeed;

    [SerializeField] protected Vector3[] myWaypoints;

    protected abstract void SetWaypointsAbstract();

    public void SetWaypoints(float _speed, float _rotationSpeed)
    {
        speed = _speed;
        rotationSpeed = _rotationSpeed;

        SetWaypointsAbstract();
    }

    public abstract void Move();
}
