using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinBitcoin : MonoBehaviour
{
    PlayerInput player;

    int score = 0;
    int n;
    public AudioSource coinSource;
    public AudioClip coinEffect;
    public GameObject[] cyberSecurityTips;
    public float GetScore { get => score; set => score = (int)value; }


    public void Awake()
    {
        player = Object.FindObjectOfType<PlayerInput>().GetComponent<PlayerInput>();

    }

    // Start is called before the first frame update
    void Start()
    {
        iTween.RotateBy(gameObject, iTween.Hash(
            "z", 360,
            "looptype", iTween.LoopType.loop,
            "speed", 60f,
            "easetype", iTween.EaseType.linear));
        Debug.Log(GetScore.ToString());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Hitman")
        {
            n = Random.Range(0,1);
            PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + 1);
            PlayerPrefs.Save();
            score = PlayerPrefs.GetInt("Score");
            Debug.Log(PlayerPrefs.GetInt("Score").ToString());
            Debug.Log(score);

            if (score == 2)
            {
                Debug.Log(n.ToString());
                cyberSecurityTips[n].SetActive(true);
                player.InputEnabled = false;
            }

            if (score == 4)
            {
                cyberSecurityTips[n].SetActive(true);
                player.InputEnabled = false;
            }

            if (score == 6)
            {
                cyberSecurityTips[n].SetActive(true);
                player.InputEnabled = false;
            }

            if (score == 8)
            {
                cyberSecurityTips[n].SetActive(true);
                player.InputEnabled = false;
            }

            coinSource.PlayOneShot(coinEffect);
            Destroy(gameObject);

        }
    }
}
