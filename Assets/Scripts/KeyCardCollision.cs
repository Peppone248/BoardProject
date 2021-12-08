using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCardCollision : MonoBehaviour
{
    Board m_board;
    public bool isDestroyed;
    private void Awake()
    {
        m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();
    }

    // Update is called once per frame
    void Update()
    {
        //m_board.DrawKey();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Hitman")
        {
            Destroy(gameObject);
            iTween.RotateTo(GameObject.Find("MetalDoor(Clone)"), iTween.Hash(
            "y", 90,
            "time", 0.7f,
            "speed", 70f,
            "easetype", iTween.EaseType.linear
            ));
        }
    }
}
