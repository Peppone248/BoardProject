using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageZoom : MonoBehaviour
{
    public Camera cameraOnBlueEnemy;
    float defaultFov = 50;
    public float zoomMultiplier = 5;
    float zoomDuration = 0.0065f;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            ZoomCamera(defaultFov / zoomMultiplier);
            StartCoroutine(myWaitCoroutine());

        } else if (cameraOnBlueEnemy.fieldOfView != defaultFov)
        {
            ZoomCamera(defaultFov);
        }
       
    }

    void ZoomCamera(float target)
    {
        float angle = Mathf.Abs((defaultFov / zoomMultiplier) - defaultFov);
        cameraOnBlueEnemy.fieldOfView = Mathf.MoveTowards(cameraOnBlueEnemy.fieldOfView, target, angle / zoomDuration * Time.deltaTime);
        Debug.Log(cameraOnBlueEnemy.fieldOfView);
        //cameraOnBlueEnemy.transform.Translate(new Vector3(1.2f, 0f, 0f));
    }

    IEnumerator myWaitCoroutine()
    {
        yield return new WaitForSeconds(3f);// Wait for one second
        gameObject.SetActive(false);
    }
    

}
