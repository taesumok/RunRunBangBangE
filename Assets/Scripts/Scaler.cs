using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scaler : MonoBehaviour
{
    public CanvasScaler thisCanvas;
    // Start is called before the first frame update
    void Start()
    {
    
        //Default �ػ� ����
        float fixedAspectRatio = 1080f / 1920f;

        //���� �ػ��� ����
        float currentAspectRatio = (float)Screen.width / (float)Screen.height;

        //���� �ػ� ���� ������ �� �� ���
        if (currentAspectRatio > fixedAspectRatio) thisCanvas.matchWidthOrHeight = 1f;
        //���� �ػ��� ���� ������ �� �� ���
        else if (currentAspectRatio < fixedAspectRatio) thisCanvas.matchWidthOrHeight = 1f;
    }

    
}
