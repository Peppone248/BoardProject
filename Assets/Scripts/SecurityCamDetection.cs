using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecurityCamDetection : MonoBehaviour
{
    Board board;
    GameManager game;
    PlayerManager player;
    public Light spotlight;
    Color g = Color.green;
    MeshRenderer hitmanRend;
    Material[] materials;
    Scene currentScene;
    string nameScene;
    bool isEnemy = false;

    private void Awake()
    {
        game = Object.FindObjectOfType<GameManager>().GetComponent<GameManager>();
        player = Object.FindObjectOfType<PlayerManager>().GetComponent<PlayerManager>();
        board = Object.FindObjectOfType<Board>().GetComponent<Board>();
    }

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        nameScene = currentScene.name;
    }

    private void Update()
    {
        hitmanRend = GameObject.Find("Hitman").GetComponent<MeshRenderer>();
        materials = hitmanRend.materials;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Hitman")
        {
            if (materials[0].name.Equals("EnemyPatrol (Instance)") && materials[4].name.Equals("EnemyPatrol (Instance)"))
            {
                
                isEnemy = true;
                
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

            else if(spotlight.color != Color.green && isEnemy == false)
            {
                Debug.Log("Sei Morto");
                player.Die();
                game.LoseLevel();
            }
                
        } else if((other.gameObject.name == "EnemyPatrol (2)" || other.gameObject.name == "EnemySentinel") && nameScene.Equals("Level4"))
        
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
        if(nameScene.Equals("Level4") && (other.gameObject.name == "EnemyPatrol (2)" || other.gameObject.name == "EnemySentinel"))
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
        else if(nameScene.Equals("Level4"))
        {
            return;
        }
    }
}
