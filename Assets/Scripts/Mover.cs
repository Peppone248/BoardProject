using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Mover : MonoBehaviour
{
    // position to reach
    public Vector3 destination;
    // face the direction of the movement by the character
    public bool faceDestination = false;
    public bool isMoving = false;
    public iTween.EaseType easeType = iTween.EaseType.easeInOutExpo;
    public float moveSpeed = 1.5f;
    public float rotateTime = 0.5f;
    public float iTweenDelay = 0f;
    protected Board m_board;
    protected Node currentNode;

    public UnityEvent finishMovementEvent;

    public Node CurrentNode { get => currentNode; }

    protected virtual void Awake()
    {
        m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();
        
    }

    protected virtual void Start()
    {
        UpdateCurrentNode();
    }
    public void Move(Vector3 destinationPos, float delayTime = 0.30f)
    {
        if (isMoving)
        {
            return;
        }
        if (m_board != null)
        {
            Node targetNode = m_board.FindNodeAt(destinationPos);
            // Check if in the list of linked nodes are present the player nodes so the player can't move to nearest nodes with no links
            if (targetNode != null && currentNode != null && currentNode.LinkedNodes.Contains(targetNode))
            {
                StartCoroutine(MoveRoutine(destinationPos, delayTime));
            }
        }
    }

    protected virtual IEnumerator MoveRoutine(Vector3 destinationPos, float delayTime)
    {
        isMoving = true;
        destination = destinationPos;

        // turn to face destination
        if (faceDestination)
        {
            FaceDestination();
            yield return new WaitForSeconds(0.15f);
        }
        yield return new WaitForSeconds(delayTime);
        
        // move toward the destinationPos
        iTween.MoveTo(gameObject, iTween.Hash(
                      "x", destinationPos.x,
                      "y", destinationPos.y,
                      "z", destinationPos.z,
                      "delay", iTweenDelay,
                      "easetype", easeType,
                      "speed", moveSpeed));

        while (Vector3.Distance(destinationPos, transform.position) > 0.01f)
        {
            yield return null;
        }

        iTween.Stop(gameObject);
        transform.position = destinationPos;
        isMoving = false;
        UpdateCurrentNode();

        //finishMovementEvent.Invoke();
    }

    public void MoveForward()
    {
        Vector3 newPosition = transform.position + new Vector3(0f, 0f, Board.spacing);
        Move(newPosition, 0);
    }

    public void MoveLeft()
    {
        Vector3 newPosition = transform.position + new Vector3(-Board.spacing, 0f, 0f);
        Move(newPosition, 0);
    }
    public void MoveRight()
    {
        Vector3 newPosition = transform.position + new Vector3(Board.spacing, 0f, 0f);
        Move(newPosition, 0);
    }
    public void MoveBackward()
    {
        Vector3 newPosition = transform.position + new Vector3(0f, 0f, -Board.spacing);
        Move(newPosition, 0);
    }

    protected void UpdateCurrentNode()
    {
        if(m_board != null)
        {
            currentNode = m_board.FindNodeAt(transform.position);
        }

    }
        
    protected void FaceDestination()
    {
        Vector3 relativePosition = destination - transform.position;
        Quaternion newRotation = Quaternion.LookRotation(relativePosition, Vector3.up);
        float newY = newRotation.eulerAngles.y;
        iTween.RotateTo(gameObject, iTween.Hash(
                        "y", newY,
                        "delay", 0f,
                        "easetype", easeType,
                        "time", rotateTime));
    }


}
