using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySensor : MonoBehaviour
{
    public Vector3 directionToSearch = new Vector3(0f, 0f, 2f);
    Node nodeToSearch;
    Board m_board;

    bool playerFound = false;
    public bool PlayerFound { get { return playerFound; } }

    private void Awake()
    {
        m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();
    }

    public void UpdateSensor(Node enemyNode)
    {
        // convert the local directionToSearch into a world space 3d position
        Vector3 worldSpacePosition = transform.TransformVector(directionToSearch) + transform.position;

        if (m_board != null)
        {
            nodeToSearch = m_board.FindNodeAt(worldSpacePosition);

           /* if (!enemyNode.LinkedNodes.Contains(nodeToSearch))
            {
                playerFound = false;
            } */

            if (nodeToSearch == m_board.PlayerNode)
            {
                playerFound = true;
            }
        }
    }
}
