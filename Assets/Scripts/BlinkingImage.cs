using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingImage : MonoBehaviour
{
    public float timer = 0.5f;
    public Image imageToBlink;
    private int count = 0;

    // Start is called before the first frame update

    private void OnEnable()
    {
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while (true)
        {
            imageToBlink.gameObject.SetActive(false);
            yield return new WaitForSeconds(timer);
            imageToBlink.gameObject.SetActive(true);
            yield return new WaitForSeconds(timer);
        }
    }

    IEnumerator SelfDestruct()
    {
        if (count == 4)
        {
            yield return new WaitForSeconds(5f);
            Destroy(gameObject);
        }
       
    }

}
