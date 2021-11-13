using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{

    public void Awake()
    {
        passwordTxt.SetActive(false);
    }
    public GameObject passwordTxt;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name=="Hitman")
        {
            Destroy(gameObject);
            passwordTxt.SetActive(true);
        }
    }

}
