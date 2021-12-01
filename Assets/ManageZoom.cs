using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageZoom : MonoBehaviour
{
    public Camera cameraOnBlueEnemy;
    public float defaultFov = 50;
    public float zoomMultiplier = 3;
    public float zoomDuration = 0.05f;

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
        }
       
    }

    void ZoomCamera(float target)
    {
        Debug.Log(target.ToString());

        float angle = Mathf.Abs((defaultFov / zoomMultiplier) - defaultFov);
        Debug.Log(angle.ToString());

        cameraOnBlueEnemy.fieldOfView = Mathf.MoveTowards(cameraOnBlueEnemy.fieldOfView, target, angle / zoomDuration * Time.deltaTime);
        Debug.Log(cameraOnBlueEnemy.fieldOfView.ToString());
    }
}
