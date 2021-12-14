using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    public Vector3 offscreenOffset = new Vector3(0f, 10f, 0f);

    Board m_board;

    public float deathDelay = 0f;
    public float offscreenDelay = 1f;

    public float iTweenDelay = 0f;
    public iTween.EaseType easeType = iTween.EaseType.easeInOutQuint;
    public float moveTime = 0.5f;
    
    private void Awake()
    {
        m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();
    }

    public void MoveOffBoard(Vector3 target)
    {
        iTween.MoveTo(gameObject, iTween.Hash(
            "x", target.x,
            "y", target.y,
            "z", target.z,
            "delay", iTweenDelay,
            "easetype", easeType,
            "time", moveTime));
    }

    public void Die()
    {
        StartCoroutine(DieRoutine());
    }

    IEnumerator DieRoutine()
    {
        yield return new WaitForSeconds(deathDelay);
        
        Vector3 offscreenPos = transform.position + offscreenOffset;
        MoveOffBoard(offscreenPos);

        yield return new WaitForSeconds(moveTime + offscreenDelay);

        if(m_board.capturePosition.Count != 0 && m_board.CurrentCapturedPosition < m_board.capturePosition.Count)
        {
            Vector3 capturePos = m_board.capturePosition[m_board.CurrentCapturedPosition].position;
            transform.position = capturePos + offscreenOffset;
            MoveOffBoard(capturePos);

            yield return new WaitForSeconds(moveTime);
            
            m_board.currentCapturedPosition++;
            m_board.currentCapturedPosition = Mathf.Clamp(m_board.CurrentCapturedPosition, 0, m_board.capturePosition.Count - 1);
        }
    }
}
