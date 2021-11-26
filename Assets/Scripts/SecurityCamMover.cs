using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCamMover : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        iTween.RotateBy(gameObject, iTween.Hash(
            "z", -0.09f,
            "looptype", iTween.LoopType.pingPong,
            "speed", 12f,
            "easetype", iTween.EaseType.linear));
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
