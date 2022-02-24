using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScore : MonoBehaviour
{
    public SpinBitcoin[] coinScore;

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Text>().text = PlayerPrefs.GetInt("Score").ToString();
    }
}
