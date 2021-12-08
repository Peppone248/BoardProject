using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCardCollision : MonoBehaviour
{
    Board m_board;
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
            Debug.Log("You got a key");
            Destroy(gameObject);
            PlayerPrefs.SetInt("key", 1);
            PlayerPrefs.Save();
        }
    }
}
