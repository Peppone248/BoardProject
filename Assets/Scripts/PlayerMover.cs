using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : Mover
{
   
    PlayerCompass playerCompass;

    protected override void Awake()
    {
        base.Awake();
        playerCompass = Object.FindObjectOfType<PlayerCompass>().GetComponent<PlayerCompass>();
    }

    protected override void Start()
    {
        base.Start();
        // set the PlayerNode in the board when level start
        UpdateBoard();
    }

    protected override IEnumerator MoveRoutine(Vector3 destinationPos, float delayTime)
    {
        
        if (playerCompass != null)
        {
            playerCompass.ShowArrows(false);
        }

        // run the rest of the method MoveRoutine
        yield return StartCoroutine(base.MoveRoutine(destinationPos, delayTime));

        UpdateBoard();
        if (playerCompass != null)
        {
            playerCompass.ShowArrows(true);
        }
    }

    public void UpdateBoard()
    {
        if(m_board != null)
        {
            m_board.UpdatePlayerNode();
        }

        base.finishMovementEvent.Invoke();
    }

}
