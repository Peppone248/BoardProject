using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayScore : MonoBehaviour
{
    public SpinBitcoin[] coinScore;
    // Start is called before the first frame update
    void Start()
    {
            gameObject.GetComponent<Text>().text = PlayerPrefs.GetInt("Score").ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
