using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    public GameObject[] pswClue1;
    public GameObject[] padlocks;
    public GameObject pauseBtn;
    PlayerInput player;
    int randomClueIndex;

    public AudioSource clueSound;
    public AudioClip clueEffect;

    public int RandomClueIndex { get => randomClueIndex; set => randomClueIndex = value; }

    public void Awake()
    {
        RandomClueIndex = Random.Range(0, pswClue1.Length);
        Debug.Log(RandomClueIndex.ToString());
        pswClue1[RandomClueIndex].SetActive(false);
        player = Object.FindObjectOfType<PlayerInput>().GetComponent<PlayerInput>();

    }
    private void OnTriggerEnter(Collider other)
    {
            if (other.gameObject.name == "Hitman")
            {
                Debug.Log(RandomClueIndex.ToString());
                pauseBtn.SetActive(false);
                clueSound.PlayOneShot(clueEffect);
                player.InputEnabled = false;
                
            if (gameObject == padlocks[0]) {
                Destroy(gameObject);
            } else if(gameObject == padlocks[1])
            {
                GameObject.Find("FirstGear").GetComponent<Renderer>().enabled = false;
                GameObject.Find("Metal_Piece").GetComponent<Renderer>().enabled = false;
                GameObject.Find("Padlock1").GetComponent<Renderer>().enabled = false;
                GameObject.Find("SecondGear").GetComponent<Renderer>().enabled = false;
                GameObject.Find("ThirdGear").GetComponent<Renderer>().enabled = false;
            }
                pswClue1[RandomClueIndex].SetActive(true);
            }
        }
}
