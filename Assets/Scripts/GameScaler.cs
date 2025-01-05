using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScaler : MonoBehaviour
{
    public GameObject g_object;
    //private GameScaler g_object;
    private float rate_x;
    private float rate_y;

    public float ScreenX;
    public float ScreenY;
    private void OnEnable()
    {
        rate_x = (float)Screen.width / ScreenX;
        rate_y = (float)Screen.height / ScreenY;


        g_object.transform.localScale = new Vector3(g_object.transform.localScale.x * rate_x, g_object.transform.localScale.y * rate_y, 1);

        Debug.Log("transform.localScale : " + g_object.transform.localScale);
    }
   

    
}
