using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamDetection : MonoBehaviour
{
    Board board;
    GameManager game;
    PlayerManager player;
    public Light spotlight;
    Color g = Color.green;

    private void Awake()
    {
        game = Object.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        player = Object.FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
        board = Object.FindObjectOfType<Board>().GetComponent<Board>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Hitman")
        {
            if(spotlight.color != Color.green)
            {
                Debug.Log("Sei Morto");
                player.Die();
                game.LoseLevel();
            }
        } else if(other.gameObject.name == "EnemyPatrol (2)")
        
        {
            iTween.RotateTo(GameObject.Find("Leaf1"), iTween.Hash(
               "y", 0f,
                "time", 0.7f,
                "speed", 50f,
                "easetype", iTween.EaseType.linear
                                                 ));

            iTween.RotateTo(GameObject.Find("Leaf2"), iTween.Hash(
                "y", 180f,
                "time", 0.7f,
                "speed", 50f,
                "easetype", iTween.EaseType.linear
                                                ));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name == "EnemyPatrol (2)" && board.FindNodeAt(GameObject.Find("EnemyPatrol (2)").transform.position).isDoorNode)
        {
            return;
        }
        else
        {
            iTween.RotateTo(GameObject.Find("Leaf1"), iTween.Hash(
               "y", 90f,
                "time", 0.7f,
                "speed", 50f,
                "easetype", iTween.EaseType.linear
                                                 ));

            iTween.RotateTo(GameObject.Find("Leaf2"), iTween.Hash(
                "y", 90f,
                "time", 0.7f,
                "speed", 50f,
                "easetype", iTween.EaseType.linear
                                                ));
        }
    }
}
