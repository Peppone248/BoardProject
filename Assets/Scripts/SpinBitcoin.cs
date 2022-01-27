using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinBitcoin : MonoBehaviour
{
    int score = 0;
    public AudioSource coinSource;
    public AudioClip coinEffect;
    public GameObject[] cyberSecurityTips;
    public float GetScore { get => score; set => score = (int)value; }


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

            PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + 1);
            PlayerPrefs.Save();
            score = PlayerPrefs.GetInt("Score");
            Debug.Log(PlayerPrefs.GetInt("Score").ToString());
            Debug.Log(score);

            if (score == 5)
            {
                Debug.Log("Entro nel ramo");
                cyberSecurityTips[0].SetActive(true);
            }

            if (score == 6)
            {
                // Show CyberSecurity Tip
            }

            if (score == 9)
            {
                // Show CyberSecurity Tip
            }

            coinSource.PlayOneShot(coinEffect);
            Destroy(gameObject);

        }
    }
}
