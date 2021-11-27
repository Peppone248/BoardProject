using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingText : MonoBehaviour
{
    public float timer = 0.5f;
    string textToBlink;
    private Text IPone;

    // Start is called before the first frame update
    void Awake()
    {
        IPone = GetComponent<Text>();
        textToBlink = IPone.text;
    }

    private void OnEnable()
    {
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while (true)
        {
            IPone.text = textToBlink;
            yield return new WaitForSeconds(timer);
            IPone.text = string.Empty;
            yield return new WaitForSeconds(timer);
        }
    }

}
