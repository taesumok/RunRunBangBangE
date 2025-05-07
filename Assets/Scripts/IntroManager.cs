using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    public Text titleText; // "�پ�� �����" �ؽ�Ʈ
    public Text touchText; // "ȭ���� ��ġ���ּ���" �ؽ�Ʈ
 

    float titleY;
    float targetTitleY;

    private float rate_x;
    private float rate_y;
    public float ScreenX;
    public float ScreenY;

    public bool isReady;

    // Start is called before the first frame update
    void Start()
    {

        rate_x = (float)Screen.width / ScreenX;
        rate_y = (float)Screen.height / ScreenY;

        // ���� �� ID�������� or ���

        

        /*
        touchText.gameObject.SetActive(false);
        StartCoroutine(BlinkTouchText());
        */
    }

 

    IEnumerator BlinkTouchText()
    {
        
        Color color = touchText.color;
        isReady = true;
        while (true) // ���� �ݺ����� ��ġ �ؽ�Ʈ ������ 
        {
            color.a = 1;
            touchText.color = color;
            yield return new WaitForSeconds(0.6f);

            color.a = 0;
            touchText.color = color;
            yield return new WaitForSeconds(0.6f);
        }

    }

}
