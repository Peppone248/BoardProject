using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLightCamera : MonoBehaviour
{

    public GameObject spotLight;
    Color g = Color.green;


    public void ChangeLightColor()
    {
        if(spotLight.GetComponent<Light>().color != g)
        {
            spotLight.GetComponent<Light>().color = g;
        } 
    }
}
