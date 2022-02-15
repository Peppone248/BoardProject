using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    public GameObject pswClue1;
    public GameObject[] padlocks;
    public GameObject pauseBtn;
    PlayerInput player;

    public AudioSource clueSound;
    public AudioClip clueEffect;

    public void Awake()
    {
        pswClue1.SetActive(false);
        player = Object.FindObjectOfType<PlayerInput>().GetComponent<PlayerInput>();

    }
    private void OnTriggerEnter(Collider other)
    {
            if (other.gameObject.name == "Hitman")
            {
                pauseBtn.SetActive(false);
                clueSound.PlayOneShot(clueEffect);
                player.InputEnabled = false;
                Destroy(gameObject);
                pswClue1.SetActive(true);
            }
        }
}
