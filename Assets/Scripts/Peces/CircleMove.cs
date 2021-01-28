using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMove : MovementModule
{
    [SerializeField] float spacingBetweenWaypoints = 2;
    [SerializeField] float radious = 3;
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

            if (Vector3.Distance(transform.position, myWaypoints[index]) < 0.2f)
            {
                inWaypont = true;
                index += 1;

                if (index >= myWaypoints.Length) index = 1;
            }
        }
    }

    protected override void SetWaypointsAbstract()
    {
        RaycastHit hit;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(0, 360), transform.eulerAngles.z);
        myWaypoints = new Vector3[8];
        Vector3 diagonalOne = transform.right + transform.forward;
        Vector3 diagonalTwo = transform.right - transform.forward;

        if (Physics.Raycast(transform.position, transform.forward, out hit, radious, mask)) myWaypoints[0] = hit.point + transform.forward * -1;
        else myWaypoints[0] = transform.position + transform.forward * radious;

        if (Physics.Raycast(transform.position, -transform.forward, out hit, radious, mask)) myWaypoints[4] = hit.point + transform.forward * 1;
        else myWaypoints[4] = transform.position - transform.forward * radious;

        if (Physics.Raycast(transform.position, transform.right, out hit, radious, mask)) myWaypoints[2] = hit.point + transform.right * -1;
        else myWaypoints[2] = transform.position + transform.right * radious;

        if (Physics.Raycast(transform.position, -transform.right, out hit, radious, mask)) myWaypoints[6] = hit.point + transform.right * 1;
        else myWaypoints[6] = transform.position - transform.right * radious;

        if (Physics.Raycast(transform.position, diagonalOne, out hit, spacingBetweenWaypoints, mask)) myWaypoints[1] = hit.point + diagonalOne * -1;
        else myWaypoints[1] = transform.position + diagonalOne * spacingBetweenWaypoints;

        if (Physics.Raycast(transform.position, -diagonalOne, out hit, spacingBetweenWaypoints, mask)) myWaypoints[5] = hit.point + diagonalOne * 1;
        else myWaypoints[5] = transform.position - diagonalOne * spacingBetweenWaypoints;

        if (Physics.Raycast(transform.position, diagonalTwo, out hit, spacingBetweenWaypoints, mask)) myWaypoints[3] = hit.point + diagonalTwo * -1;
        else myWaypoints[3] = transform.position + diagonalTwo * spacingBetweenWaypoints;

        if (Physics.Raycast(transform.position, -diagonalTwo, out hit, spacingBetweenWaypoints, mask)) myWaypoints[7] = hit.point + diagonalTwo * 1;
        else myWaypoints[7] = transform.position - diagonalTwo * spacingBetweenWaypoints;

        index = 0;
        inWaypont = false;
        timer = 0;
    }
}
