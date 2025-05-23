using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


public class DropSpawner : MonoBehaviour
{
    public static DropSpawner instance;
    // �� �������� �����ϴ� ����

    private GameObject orgDropPrefeb;

   

    public GameObject dropPrefab_1;
    public GameObject dropPrefab_2;
    public GameObject dropPrefab_3;

    public GameObject spaceShipPrefab;

    public GameObject itemPrefab;
    public GameObject item2Prefab;


    private float rate_x;
    private float rate_y;
    public float ScreenX;
    public float ScreenY;

    // ���� �����Ǵ� ����(��)�� �����ϴ� ����
    public float spawnRate;

    public float spaceSpawnRate;

    public float itemSpawnRate;


    public int spawnCount;

    // ���� Ÿ�̸Ӹ� �����ϴ� ����
    public float timer;
    public float shipTimer;
    public float itemTimer;
    public bool doSpawn = true;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("�� �ΰ� �̻��� instance �� �����մϴ�(DropSpwner)");
            Destroy(instance);
        }
    }

    void Start()
    {
        spawnCount = 0;
        orgDropPrefeb = dropPrefab_1;

        rate_x = (float)Screen.width / ScreenX;
        rate_y = (float)Screen.height / ScreenY;
       
    }

    void Update()
    {
        // �� �����Ӹ��� Ÿ�̸ӿ� �ð� �߰�
       

        // Ÿ�̸Ӱ� ���� ������ �ʰ����� ��
        if (doSpawn)
        {
            timer += Time.deltaTime;
            shipTimer += Time.deltaTime;
            itemTimer += Time.deltaTime;

            // 운석 생성
            if (timer >= spawnRate / 10)
            {
                // ���� �����ϴ� �Լ� ȣ��
                SpawnDrop();
                spawnCount++;
               // Debug.Log("spawnCount : " + spawnCount);

                // Ÿ�̸Ӹ� �ʱ�ȭ
                timer = 0;
            }

            // 우주선 생성
            if (shipTimer >= spaceSpawnRate / 10)
            {
                // ���� �����ϴ� �Լ� ȣ��
                SpawnSpaceShip();
               // Debug.Log("spawnCount : " + spawnCount);

                // Ÿ�̸Ӹ� �ʱ�ȭ
                shipTimer = 0;
            }

            // Item 생성
            if (itemTimer >= itemSpawnRate / 10)
            {
                // ���� �����ϴ� �Լ� ȣ��
                SpawnItem();
               // Debug.Log("spawnCount : " + spawnCount);

                // Ÿ�̸Ӹ� �ʱ�ȭ
                itemTimer = 0;
            }

            
        }

        
        
    }

    void SpawnItem(){
          // ������ x ��ġ�� ���� (-8���� 8 ����)
        GameObject drop;
        float xPosition = Random.Range(-3.35f* rate_x, 3.35f* rate_x) ;

        // ���� ��ġ�� ���� (���� x ��ġ, ���� ������Ʈ�� y ��ġ)
        Vector3 spawnPosition = new Vector3(xPosition, transform.position.y, 0);

        // �� �������� ���� ��ġ�� �ν��Ͻ�ȭ
        int prefab = Random.Range(1,3);

        Debug.Log("prefab : " + prefab );

        if(prefab == 1){
            drop = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        }
        else{
            drop = Instantiate(item2Prefab, spawnPosition, Quaternion.identity);
        }
        
        drop.transform.localScale = new Vector3(drop.transform.localScale.x * rate_x , drop.transform.localScale.y * rate_y , 1);

    }
    void SpawnSpaceShip()
    {
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y, 0);
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");   
        if(playerObject != null){ 
            spawnPosition = new Vector3(playerObject.transform.position.x, transform.position.y, 0);
        }

        GameObject spaceShip = Instantiate(spaceShipPrefab, spawnPosition, Quaternion.identity);
        spaceShip.transform.localScale = new Vector3(spaceShip.transform.localScale.x * rate_x , spaceShip.transform.localScale.y * rate_y , 1);
    }
    

    

    void SpawnDrop()
    {
        // ������ x ��ġ�� ���� (-8���� 8 ����)
        float xPosition = Random.Range(-3.35f* rate_x, 3.35f* rate_x) ;

        // ���� ��ġ�� ���� (���� x ��ġ, ���� ������Ʈ�� y ��ġ)
        Vector3 spawnPosition = new Vector3(xPosition, transform.position.y, 0);

        // �� �������� ���� ��ġ�� �ν��Ͻ�ȭ
        
        GameObject drop = Instantiate(dropPrefab_1, spawnPosition, Quaternion.identity);
        
        drop.transform.localScale = new Vector3(drop.transform.localScale.x * rate_x , drop.transform.localScale.y * rate_y , 1);
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