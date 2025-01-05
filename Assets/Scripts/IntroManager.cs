using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    public Text titleText; // "뛰어라 방방이" 텍스트
    public Text touchText; // "화면을 터치해주세요" 텍스트
 

    float titleY;
    float targetTitleY;

    private float rate_x;
    private float rate_y;
    public float ScreenX;
    public float ScreenY;

    public bool isReady = false;

    // Start is called before the first frame update
    void Start()
    {

        rate_x = (float)Screen.width / ScreenX;
        rate_y = (float)Screen.height / ScreenY;

        // 접속 시 ID가져오기 or 등록

        


        touchText.gameObject.SetActive(true);
        StartCoroutine(BlinkTouchText());
    }

 

    IEnumerator BlinkTouchText()
    {
        
        Color color = touchText.color;
        isReady = true;
        while (true) // 무한 반복으로 터치 텍스트 깜빡임 
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
