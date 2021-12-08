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
        m_board.ChangeCameraOnNode();
        if (m_board.StopPlayerOnDoor())
        {
            iTween.RotateTo(gameObject, iTween.Hash(
            "y", -90,
            "time", 0.7f,
            "speed", speed,
            "easetype", iTween.EaseType.linear
        ));
        }
        
        if (m_board.StopPlayerOnMetalDoor())
        {
            //OpenDoorWithKey();
        }
    }

    public void OpenDoorWithKey()
    {
        Debug.Log(PlayerPrefs.GetInt("key").ToString());
        if (PlayerPrefs.GetInt("key") == 1)
        {
            iTween.RotateTo(gameObject, iTween.Hash(
            "y", -90,
            "time", 0.7f,
            "speed", speed,
            "easetype", iTween.EaseType.linear
        ));
        }
    }
}
