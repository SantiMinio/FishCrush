using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MovementModule
{
    [SerializeField] float radiousToSearchWaypoints = 4;
    [SerializeField] int waypointsAmmount = 4;
    [SerializeField] float timeToNextWaypoint = 1;
    [SerializeField] LayerMask mask = 1 << 0;

    float timer;
    [SerializeField] int index;
    bool inWaypont = false;

    Vector3 currentWaypoint;

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
            transform.forward = Vector3.Lerp(transform.forward, (currentWaypoint - transform.position).normalized, Time.deltaTime * rotationSpeed);
            transform.position += transform.forward * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, myWaypoints[index]) < 0.5f)
            {
                int indexRandom = Random.Range(0, myWaypoints.Length);
                Vector3 temp = myWaypoints[indexRandom];
                myWaypoints[indexRandom] = currentWaypoint;
                currentWaypoint = temp;
                inWaypont = true;
            }
        }
    }

    protected override void SetWaypointsAbstract()
    {
        myWaypoints = new Vector3[waypointsAmmount - 1];

        float zMin = transform.position.z - radiousToSearchWaypoints;
        float zMax = transform.position.z + radiousToSearchWaypoints;
        float xMin = transform.position.x - radiousToSearchWaypoints;
        float xMax = transform.position.x + radiousToSearchWaypoints;

        for (int i = 0; i < waypointsAmmount; i++)
        {
            if (i >= myWaypoints.Length) { currentWaypoint = new Vector3(Random.Range(xMin, xMax), transform.position.y, Random.Range(zMin, zMax)); break; }
            myWaypoints[i] = new Vector3(Random.Range(xMin, xMax), transform.position.y, Random.Range(zMin, zMax));
        }

        index = 0;
        inWaypont = false;
        timer = 0;
    }
}
