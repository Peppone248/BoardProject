using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDetect : MonoBehaviour
{
    Board m_board;
    public int speed=50;
    private void Awake()
    {
        m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_board.ChangeCameraOnNodeLvl3();
        if (m_board.StopPlayerOnDoor())
        {
            iTween.RotateTo(gameObject, iTween.Hash(
            "y", -90,
            "time", 0.7f,
            "speed", speed,
            "easetype", iTween.EaseType.linear
        ));
        }
    }

    public void OpenDoorWithKey()
    {
        iTween.RotateTo(gameObject, iTween.Hash(
            "y", 90,
            "time", 0.7f,
            "speed", speed,
            "easetype", iTween.EaseType.linear
            ));
    }

    public void OpenDoubleDoor()
    {
        iTween.RotateTo(gameObject, iTween.Hash(
            "y", 90,
            "time", 0.7f,
            "speed", speed,
            "easetype", iTween.EaseType.linear
            ));
    }

}
