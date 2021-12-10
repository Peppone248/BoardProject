using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingImage : MonoBehaviour
{
    // this is the UI.Text or other UI element you want to toggle
    public MaskableGraphic imageToToggle;
    public float interval = 1f;
    public float startDelay = 0.5f;
    public float counter;
    public bool currentState = true;
    public bool defaultState = true;
    bool isBlinking = false;


    void Start()
    {
        counter = 6;
        imageToToggle.enabled = defaultState;
        StartBlink();
    }

    private void Update()
    {
        imageToToggle.gameObject.SetActive(true);
        if (counter <= 0)
        {
            CancelInvoke("ToggleState");
            imageToToggle.gameObject.SetActive(false);
        }
    }

    public void StartBlink()
    {
        // do not invoke the blink twice - needed if you need to start the blink from an external object
        if (isBlinking)
            return;

        if (imageToToggle != null)
        {
            isBlinking = true;
            InvokeRepeating("ToggleState", startDelay, interval);
            InvokeRepeating("ToggleState", startDelay, interval);
        }
    }

    public void ToggleState()
    {
        imageToToggle.enabled = !imageToToggle.enabled;
        counter--;
    }

}
