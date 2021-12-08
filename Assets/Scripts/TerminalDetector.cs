using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalDetector : MonoBehaviour
{
    Board m_board;

    private void Awake()
    {
        m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_board.StopPlayerOnTerminal())
        {
            Debug.Log("Hai hackerato le cam");
        }
    }
}
