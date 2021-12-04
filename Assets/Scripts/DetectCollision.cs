using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    public GameObject pswClue1;
    public GameObject[] padlocks;
    PlayerInput player;
    EnemySensor playerDetection;
    //public bool isDiscover1=false;

    public void Awake()
    {
        pswClue1.SetActive(false);
        player = Object.FindObjectOfType<PlayerInput>().GetComponent<PlayerInput>();
        playerDetection = Object.FindObjectOfType<EnemySensor>().GetComponent<EnemySensor>();

    }
    private void OnTriggerEnter(Collider other)
    {
            if (other.gameObject.name == "Hitman")
            {
                player.InputEnabled = false;
                Destroy(gameObject);
                pswClue1.SetActive(true);
            }
        }
}
