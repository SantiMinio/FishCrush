using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMove : MovementModule
{
    [SerializeField] float spacingBetweenWaypoints = 2;
    [SerializeField] float timeToNextWaypoint = 1;
    [SerializeField] LayerMask mask = 1 << 0;

    float timer;
    [SerializeField] int index;
    bool inWaypont = false;

    public override void Move()
    {
        if (inWaypont)
        {
            timer += Time.deltaTime;
            if (timer >= timeToNextWaypoint)
            {
                timer = 0;
                inWaypont = false;
            }
        }
        else
        {
            transform.forward = Vector3.Lerp(transform.forward, (myWaypoints[index] - transform.position).normalized, Time.deltaTime * rotationSpeed);
            transform.position += transform.forward * speed * Time.deltaTime;

            if(Vector3.Distance(transform.position, myWaypoints[index]) < 0.2f)
            {
                inWaypont = true;
                if (index == 0) index = 1;
                else index = 0;
            }
        }
    }

    protected override void SetWaypointsAbstract()
    {
        float[] sign = new float[2] { -1, 1 };

        float x = Random.Range(0.5f, 1) * sign[Random.Range(0, 2)];
        float z = Random.Range(0.5f, 1) * sign[Random.Range(0, 2)];
        Vector3 dir = new Vector3(x, 0, z).normalized;

        RaycastHit hit;
        Vector3 initialWaypoint;
        Vector3 secondWaypoint;

        if (Physics.Raycast(transform.position, dir, out hit, spacingBetweenWaypoints, mask))
        {
            initialWaypoint = hit.point + dir * -1;
        }
        else
        {
            initialWaypoint = transform.position + dir * spacingBetweenWaypoints;
        }

        if (Physics.Raycast(transform.position, -dir, out hit, spacingBetweenWaypoints, mask))
        {
            secondWaypoint = hit.point + dir * 1;
        }
        else
        {
            secondWaypoint = transform.position - dir * spacingBetweenWaypoints;
        }

        myWaypoints = new Vector3[2] { initialWaypoint, secondWaypoint };
        index = 0;
        inWaypont = false;
        timer = 0;
    }
}
