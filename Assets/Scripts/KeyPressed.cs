using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyPressed : MonoBehaviour
{
    public bool activateOnStart = true;
    bool moveTutorialEnded = false;
    bool blueEnemyTutorialEnded = false;
    int countKeyPress = 0;
    public float activationDelay = 3.0f;
    public Image blueEnemyDescription;
    public Image orangeEnemyDescription;
    public GameObject arrow;
    
    // Start is called before the first frame update
    void Start()
    {
        blueEnemyDescription.gameObject.SetActive(false);
        orangeEnemyDescription.gameObject.SetActive(false);
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
            countKeyPress++;
            gameObject.SetActive(false);
            blueEnemyDescription.gameObject.SetActive(true);
            Instantiate(arrow, new Vector3(2f, 3f, 2f), Quaternion.Euler(-182.80f, -290.347f, 276.406f));
            moveTutorialEnded = true;
          }

       /* if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.UpArrow)
            || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) && moveTutorialEnded == true && countKeyPress==1)
        {
            blueEnemyDescription.gameObject.SetActive(false);
            orangeEnemyDescription.gameObject.SetActive(true);
            iTween.MoveTo(GameObject.Find("Arrow5(Clone)"), iTween.Hash(
                "x", 0f,
                "y", 3f,
                "z", 0f,
                "speed", 20f,
                "time", 1f,
                "easetype", iTween.EaseType.linear));
        } */
    }

    public void ActivateDelayed()
    {
        Invoke(nameof(AfterDelay), activationDelay);
    }

    private void AfterDelay()
    {
        gameObject.SetActive(true);
    }

    private void AfterDelay(GameObject arrowInstance)
    {
        Destroy(arrowInstance);
        blueEnemyDescription.gameObject.SetActive(false);
    }

    public void ActivateDelayed(float customDelay)
    {
        Invoke(nameof(AfterDelay), customDelay);
    }
}
