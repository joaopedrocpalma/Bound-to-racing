using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class Path_controller : MonoBehaviour
{
    public Color lineColor;

    private List<Transform> nodes = new List<Transform>();  // Creates a list of transforms

    void OnDrawGizmosSelected()
    {
        Gizmos.color = lineColor;   // Draws a line between all nodes in the path

        Transform[] pathTransforms = GetComponentsInChildren<Transform>();  // Gets all the children in the path GameObject and stores them in the list of pathTransforms

        nodes = new List<Transform>();

        Vector3 currentNode;
        Vector3 prevNode = Vector3.zero;


        for (int i = 0; i < pathTransforms.Length; i++) // Checks for each individual transform in the pathTransforms
        {
            if(pathTransforms[i] != transform)  // If it's not the repeated transform
            {
                nodes.Add(pathTransforms[i]);   // Adds the Transform to the node list, thus creating the path
            }
        }

        for (int i = 0; i < nodes.Count; i++)   // Checks all nodes to store the positions
        {
            currentNode = nodes[i].position;    // Always saves the current position in a node

            if (i > 0)  // If index is higher than 0
            {
                prevNode = nodes[i - 1].position;   // Stores the previous node
            }
            else if(i == 0 && nodes.Count > 1)  // If there are less than 1 nodes and index is at 0
            {
                prevNode = nodes[nodes.Count - 1].position; // Stores the last node (or the previous before 0)
            }

            Gizmos.DrawLine(prevNode, currentNode);
            Gizmos.DrawWireSphere(currentNode,2);
        }
    }
}
