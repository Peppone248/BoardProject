using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinBitcoin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        iTween.RotateBy(gameObject, iTween.Hash(
            "z", 360,
            "looptype", iTween.LoopType.loop,
            "speed", 60f,
            "easetype", iTween.EaseType.linear));
    }
}
