using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Device;


public class CameraScaler : MonoBehaviour
{
    private float rate_x;
    private float rate_y;
    public float baseWidth;
    public float baseHeight;
    private void Start()
    {
        Camera camera = GetComponent<Camera>();
        //Rect rect = camera.rect;

        rate_x = (float)Screen.width / baseWidth;
        rate_y = (float)Screen.height / baseHeight;

        /*
        float scaleheight = ((float)Screen.width / Screen.height) / (baseWidth/baseHeight);
        float scaleWidth = 1f / scaleheight;

        if(scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            rect.width = scaleWidth;
            rect.x = (1f - scaleWidth) / 2f;

        }
        */
        camera.orthographicSize *= rate_y;
        //camera.rect = rect; 
    }
    //private void OnPreCull()=>GL.Clear(true, true, Color.black);
}
