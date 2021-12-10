using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalDetector : MonoBehaviour
{
    Board m_board;
    public GameObject spoofAlert;

    private void Awake()
    {
        m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();
        spoofAlert = GameObject.Find("SpoofAlert");
    }

    // Update is called once per frame
    void Update()
    {
        if (m_board.StopPlayerOnTerminal())
        {
            m_board.spoofed.gameObject.SetActive(true);
        }
    }
}
