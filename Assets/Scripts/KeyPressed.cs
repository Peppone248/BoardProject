using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPressed : MonoBehaviour
{

    public bool ActivateOnStart = true;
    public float ActivationDelay = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        if (ActivateOnStart) 
            ActivateDelayed();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow) 
            || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            gameObject.SetActive(false);
        }
    }

    public void ActivateDelayed()
    {
        Invoke(nameof(AfterDelay), ActivationDelay);
    }

    private void AfterDelay()
    {
        gameObject.SetActive(true);
    }

    public void ActivateDelayed(float customDelay)
    {
        Invoke(nameof(AfterDelay), customDelay);
    }

}
