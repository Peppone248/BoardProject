using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    public GameObject pswClue1;
    public GameObject[] padlocks;
    //public bool isDiscover1=false;

    public void Awake()
    {
        pswClue1.SetActive(false);

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Hitman")
            {
                Destroy(gameObject);
                pswClue1.SetActive(true);
                //isDiscover1 = true;
            } 
        }

}
