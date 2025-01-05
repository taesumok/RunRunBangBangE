using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DropSpawner : MonoBehaviour
{
    public static DropSpawner instance;
    // 똥 프리팹을 설정하는 변수

    private GameObject orgDropPrefeb;

    public GameObject dropPrefab_1;
    public GameObject dropPrefab_2;
    public GameObject dropPrefab_3;


    private float rate_x;
    private float rate_y;
    public float ScreenX;
    public float ScreenY;

    // 똥이 스폰되는 간격(초)을 설정하는 변수
    public float spawnRate;

    // 스폰 타이머를 저장하는 변수
    private float timer;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("씬 두개 이상의 instance 가 존재합니다(DropSpwner)");
            Destroy(instance);
        }
    }

    void Start()
    {
        orgDropPrefeb = dropPrefab_1;

        rate_x = (float)Screen.width / ScreenX;
        rate_y = (float)Screen.height / ScreenY;
       
    }

    void Update()
    {
        // 매 프레임마다 타이머에 시간 추가
        timer += Time.deltaTime;

        // 타이머가 스폰 간격을 초과했을 때
        if (timer >= spawnRate/10)
        {
            // 똥을 스폰하는 함수 호출
            SpawnDrop();

            // 타이머를 초기화
            timer = 0;
        }
    }

    void SpawnDrop()
    {
        // 랜덤한 x 위치를 생성 (-8에서 8 사이)
        float xPosition = Random.Range(-3.35f* rate_x, 3.35f* rate_x) ;

        // 스폰 위치를 설정 (랜덤 x 위치, 현재 오브젝트의 y 위치)
        Vector3 spawnPosition = new Vector3(xPosition, transform.position.y, 0);

        // 똥 프리팹을 스폰 위치에 인스턴스화
        GameObject drop = Instantiate(dropPrefab_1, spawnPosition, Quaternion.identity);
        
        drop.transform.localScale = new Vector3(drop.transform.localScale.x * rate_x, drop.transform.localScale.y * rate_y, 1);
        //sDebug.Log("drop.transform.localScale.x : " + drop.transform.localScale.x + " drop.transform.localScale.y : " + drop.transform.localScale.y);
    }
    public void ChangeDrop(int level)
    {
        switch (level%3)
        {
            case 0:
                orgDropPrefeb = dropPrefab_3;
                break;
            case 1:
                orgDropPrefeb = dropPrefab_1;
                break;
            case 2:
                orgDropPrefeb = dropPrefab_2;
                break;
           

            default:
                orgDropPrefeb = dropPrefab_1;
                break;
        }
    }

    void OnDisable()
    {
        GameObject[] drops = GameObject.FindGameObjectsWithTag("Drop");
        foreach (GameObject drop in drops)
        {
            Destroy(drop);
        }
    }
}