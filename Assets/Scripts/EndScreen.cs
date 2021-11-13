using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class EndScreen : MonoBehaviour
{
    public PostProcessProfile blurProf;
    public PostProcessProfile normalProf;
    public PostProcessVolume postProcessVolume;

    public void EnableCameraBlur(bool state)
    {
        if (postProcessVolume != null && blurProf != null && normalProf != null)
        {
            if (state)
            {
                Invoke("AssignVolume", 1f);
                
            }
            else
                postProcessVolume.profile = normalProf;
        }
    }

    public void AssignVolume()
    {
        postProcessVolume.profile = blurProf;
    }



}
