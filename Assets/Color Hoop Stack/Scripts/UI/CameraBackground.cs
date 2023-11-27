using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBackground : MonoBehaviour
{
    public Camera cam;
    public Vector2 defaultCamSizeRatio;

    private void Start()
    { 
        float defaultCamAspect = defaultCamSizeRatio.x / defaultCamSizeRatio.y;
        if (((float)Screen.width / (float)Screen.height) > defaultCamAspect)
        {
            //fit brackground to 4:3 ratio screen and etc...
            cam.orthographicSize = cam.orthographicSize / (defaultCamSizeRatio.y / defaultCamSizeRatio.x) /cam.aspect;
        }
    }
}
