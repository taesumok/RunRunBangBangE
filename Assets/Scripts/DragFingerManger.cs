using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class DragFingerManger : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();   
        
        StartCoroutine(Disappear());
    }

    // Update is called once per frame

    IEnumerator Disappear()
    {
        float move_x = -0.5f;
        Color color = spriteRenderer.color;
        //Debug.Log("color.a : " + color.a);
        while (color.a <= 1) // ���� �ݺ����� ��ġ �ؽ�Ʈ ������ 
        {
            if (transform.position.x <= -1.0f || transform.position.x >= 1.0f)
            {
                move_x *= -1;
            }
            transform.position = new Vector3(transform.position.x + move_x, transform.position.y, 0);
            
            
            
            //Debug.Log("color.a : " + color.a);
            color.a -= 0.1f;
            spriteRenderer.color = color;
            yield return new WaitForSeconds(0.1f);

        }

       
       
        gameObject.SetActive(false);

    }
}