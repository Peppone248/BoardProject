using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinBitcoin : MonoBehaviour
{
    float score = 0;
    public AudioSource coinSource;
    public AudioClip coinEffect;

    public float GetScore { get => score; set => score = value; }


    // Start is called before the first frame update
    void Start()
    {
        iTween.RotateBy(gameObject, iTween.Hash(
            "z", 360,
            "looptype", iTween.LoopType.loop,
            "speed", 60f,
            "easetype", iTween.EaseType.linear));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Hitman")
        {
            coinSource.PlayOneShot(coinEffect);
            Destroy(gameObject);
            score++;

            switch (score)
            {
                case 0:
                    if (score == 3)
                    {
                        // Show CyberSecurity Tip
                    }
                    break;

                case 1:
                    if (score == 6)
                    {
                        // Show CyberSecurity Tip
                    }
                    break;

                case 2:
                    if (score == 9)
                    {
                        // Show CyberSecurity Tip
                    }
                    break;
            }
        }
    }
}
