using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamDetection : MonoBehaviour
{

    GameManager game;
    PlayerManager player;
    public Light spotlight;
    Color g = Color.green;

    private void Awake()
    {
        game = Object.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        player = Object.FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
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
        }
    }
}
