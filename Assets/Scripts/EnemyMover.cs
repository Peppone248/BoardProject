using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType
{
    Stationary,
    Patrol,
    Spinner
}
public class EnemyMover : Mover
{
    public Vector3 directionToMove = new Vector3(0f, 0f, Board.spacing);
    public MovementType movementType = MovementType.Stationary;
    public float speedMovement;

    public float standTime = 1f;
    // Start is called before the first frame update

    protected override void Awake()
    {
        base.Awake();
        faceDestination = true;
    }

    protected override void Start()
    {
        base.Start();
       // StartCoroutine(TestMovementRoutine());
    }

    public void MoveOneTurn()
    {
        switch (movementType)
        {
            case MovementType.Stationary:
                Stand();
                break;
            case MovementType.Patrol:
                Patrol();
                break;
            case MovementType.Spinner:
                Spin();
                break;
        } 
    }

    void Patrol()
    {
        StartCoroutine(PatrolRoutine());
    }

    IEnumerator PatrolRoutine()
    {
        Vector3 startPosition = new Vector3(currentNode.Coordinate.x, 0f, currentNode.Coordinate.y);
        
        // where the enemy will move on. The startPosition on the board + the 2-unit direction
        Vector3 destinationPosition = transform.TransformVector(directionToMove) + startPosition;
        
        // two space moving on
        Vector3 nextDestination = startPosition + transform.TransformVector(directionToMove*2);
        
        Move(destinationPosition, 0f);
        
        while (isMoving)
        {
            yield return null;
        }

        if(m_board != null)
        {
            Node destinationNode = m_board.FindNodeAt(destinationPosition);
            Node nextDestinationNode = m_board.FindNodeAt(nextDestination);
            
            if(nextDestinationNode == null || !destinationNode.LinkedNodes.Contains(nextDestinationNode))
            {
                destination = startPosition;
                FaceDestination();
                yield return new WaitForSeconds(rotateTime);
            }
        }
        base.finishMovementEvent.Invoke();
    }

    void Stand()
    {
        StartCoroutine(StandRoutine());
    }

    IEnumerator StandRoutine()
    {
        yield return new WaitForSeconds(standTime);
        base.finishMovementEvent.Invoke();
    }

    void Spin()
    {
        StartCoroutine(SpinRoutine());
    }

    IEnumerator SpinRoutine()
    {
        Vector3 localForward = new Vector3(0f, 0f, Board.spacing);
        destination = transform.TransformVector(localForward * -1f) + transform.position;

        FaceDestination();
        yield return new WaitForSeconds(rotateTime);
        
        base.finishMovementEvent.Invoke();
    }
}
