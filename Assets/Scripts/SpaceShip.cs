using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;

public class SpaceShip : MonoBehaviour
{
    // Start is called before the first frame update
    public float shipMoveSpeed = 0.5f;
    public GameObject spaceDrop;

    private float rate_x;
    private float rate_y;
    public float ScreenX;
    public float ScreenY;
    
    void Start()
    {
        rate_x = (float)Screen.width / ScreenX;
        rate_y = (float)Screen.height / ScreenY;
        StartCoroutine(doShipMoveAndShoot());
    }

    IEnumerator doShipMoveAndShoot(){
        yield return StartCoroutine(goDown());
    }

    IEnumerator goDown(){


        while(true){
            if(transform.position.y < 5.0f*rate_y){
                yield return StartCoroutine(doShoot());
            }
            transform.position = new Vector3(transform.position.x, transform.position.y - shipMoveSpeed, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator doShoot(){

        yield return new WaitForSeconds(0.5f);
        Instantiate(spaceDrop, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(goUp());

    }
    IEnumerator goUp(){
        while(true){
            if(transform.position.y > 7.5f*rate_y){
                yield return new WaitForSeconds(1f);
                Destroy(this.gameObject);
            }
            transform.position = new Vector3(transform.position.x, transform.position.y + shipMoveSpeed, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }
    
}
