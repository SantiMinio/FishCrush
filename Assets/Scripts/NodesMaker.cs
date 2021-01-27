using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class NodesMaker : MonoBehaviour
{
    [SerializeField] Node nodePrefab = null;
    [SerializeField] float spacingBetweenNodes = 1.5f;
    [SerializeField] float nodesAmmount = 15;
    [SerializeField] Transform centerPoint = null;

    [SerializeField] bool createNodes = false;
    public FishSpawner spawner;

    private void Update()
    {
        if (!createNodes) return;

        spawner.myNodes.Add(NodePosition.Up, new List<Node>());
        spawner.myNodes.Add(NodePosition.Down, new List<Node>());
        spawner.myNodes.Add(NodePosition.Right, new List<Node>());
        spawner.myNodes.Add(NodePosition.Left, new List<Node>());
        createNodes = false;

        float total = nodesAmmount + nodesAmmount;

        var oldNodes = GetComponentsInChildren<Node>();

        for (int i = 0; i < oldNodes.Length; i++)
            DestroyImmediate(oldNodes[i].gameObject);

        Vector3 centerPointAdjust = new Vector3(centerPoint.position.x, 5, centerPoint.position.z);

        Vector3 startPosition = new Vector3(centerPointAdjust.x + nodesAmmount * spacingBetweenNodes, centerPointAdjust.y, centerPointAdjust.z + nodesAmmount * spacingBetweenNodes);

        float zPos = startPosition.z;
        float xPos = startPosition.x;
        float yPos = startPosition.y;
        RaycastHit hit;

        for (int z = 0; z < total; z++)
        {
            for (int x = 0; x < total; x++)
            {
                Vector3 raycastPoint = new Vector3(xPos, yPos, zPos);
                if(Physics.Raycast(raycastPoint, -transform.up, out hit, 10))
                {
                    if(hit.transform.gameObject.layer == 10)
                    {
                        float upDown = Mathf.Abs(zPos - centerPointAdjust.z);
                        float rightLeft = Mathf.Abs(xPos - centerPointAdjust.x);
                        var node = Instantiate(nodePrefab, transform);
                        node.transform.position = hit.point;
                        node.name = z + "" + x;

                        if (rightLeft > upDown)
                        {
                            if (xPos - centerPointAdjust.x < -1) node.myPosition = NodePosition.Left;
                            else node.myPosition = NodePosition.Right;
                        }
                        else
                        {
                            if (zPos - centerPointAdjust.z < -1) node.myPosition = NodePosition.Down;
                            else node.myPosition = NodePosition.Up;
                        }

                        spawner.myNodes[node.myPosition].Add(node);
                    }
                }
                xPos -= spacingBetweenNodes;
            }
            xPos = startPosition.x;
            zPos -= spacingBetweenNodes;
        }

    }
}
