using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class SkyManager : MonoBehaviour
{
    float moveY = 0.001f;
    float org_positioyY;
    // Start is called before the first frame update
    void Start()
    {
        org_positioyY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - moveY, 0);
        if(transform.position.y <= -4.3f){
             transform.position = new Vector3(transform.position.x, org_positioyY, 0);
        }


    }
}