using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCardCollision : MonoBehaviour
{
    Board m_board;
    GameManager game;
    PlayerMover m_player;
    PlayerManager playerManager;
    public GameObject obstacleOnDoor;
    bool isClosed = true;
    private void Awake()
    {
        m_board = Object.FindObjectOfType<Board>().GetComponent<Board>();
        game = Object.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        m_player = Object.FindObjectOfType<PlayerMover>().GetComponent<PlayerMover>();
        playerManager = Object.FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_board.FindNodeAt(m_player.transform.position).isDoorNode && isClosed)
        {
            playerManager.Die();
            game.LoseLevel();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Hitman")
        {
            isClosed = false;
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
