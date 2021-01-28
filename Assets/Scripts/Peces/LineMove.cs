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

            if(Vector3.Distance(transform.position, myWaypoints[index]) < 0.5f)
            {
                inWaypont = true;
                if (index == 0) index = 1;
                else index = 0;
            }
        }
    }

    protected override void SetWaypointsAbstract()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(0, 360), transform.eulerAngles.z);

        RaycastHit hit;
        Vector3 initialWaypoint;
        Vector3 secondWaypoint;

        if (Physics.Raycast(transform.position, transform.forward, out hit, spacingBetweenWaypoints, mask))
        {
            initialWaypoint = hit.point + transform.forward * -1;
            Debug.Log("golpea?");
        }
        else
        {
            initialWaypoint = transform.position + transform.forward * spacingBetweenWaypoints;
        }

        if (Physics.Raycast(transform.position, -transform.forward, out hit, spacingBetweenWaypoints, mask))
        {
            Debug.Log("golpea?");
            secondWaypoint = hit.point + transform.forward * 1;
        }
        else
        {
            secondWaypoint = transform.position - transform.forward * spacingBetweenWaypoints;
        }

        myWaypoints = new Vector3[2] { initialWaypoint, secondWaypoint };
        index = 0;
        inWaypont = false;
        timer = 0;
    }
}
