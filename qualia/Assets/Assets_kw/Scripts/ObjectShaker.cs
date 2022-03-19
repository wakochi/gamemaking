using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ObjectShaker : MonoBehaviour
{
    //public void CameraInpulse()
    //{   
    //    var source = GetComponent<CinemachineImpulseSource>();
    //    source.GenerateImpulse();
    //    Debug.Log("aaa");
    //}

    public void Shake(GameObject shakeObj)
    {
        iTween.ShakePosition(shakeObj, iTween.Hash("x", 0.3f, "y", 0.3f, "time", 1.4f));
    }

}
