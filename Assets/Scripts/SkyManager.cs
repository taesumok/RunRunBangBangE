using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyManager : MonoBehaviour
{
    public static SkyManager instance;
    float org_positioyY;

    private float rate_x;
    private float rate_y;

    public float ScreenX;
    public float ScreenY;

    public SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) { 
            instance = this;
        }
        else{
            Destroy(gameObject);
        }
        rate_x = (float)Screen.width / ScreenX;
        rate_y = (float)Screen.height / ScreenY;
        spriteRenderer = GetComponent<SpriteRenderer>();
        //org_positioyY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        transform.position = new Vector3(transform.position.x, transform.position.y - moveY  *Time.deltaTime * rate_y, 0);
        if(transform.position.y <= -3.7f* rate_y)
        {
             transform.position = new Vector3(transform.position.x, org_positioyY, 0);
        }
        */


    }
}
