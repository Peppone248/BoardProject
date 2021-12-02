using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyPressed : MonoBehaviour
{
    public bool activateOnStart = true;
    public bool moveTutorialEnded = false;
    public float activationDelay = 2.0f;
    public Image blueEnemyDescription;
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        if (activateOnStart) 
            ActivateDelayed();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow) 
            || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
          {
            gameObject.SetActive(false);
            blueEnemyDescription.gameObject.SetActive(true);
            moveTutorialEnded = true;
          } 
    }

    public void ActivateDelayed()
    {
        Invoke(nameof(AfterDelay), activationDelay);
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
