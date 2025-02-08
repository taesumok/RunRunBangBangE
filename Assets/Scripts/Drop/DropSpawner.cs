using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DropSpawner : MonoBehaviour
{
    public static DropSpawner instance;
    // �� �������� �����ϴ� ����

    private GameObject orgDropPrefeb;

   

    public GameObject dropPrefab_1;
    public GameObject dropPrefab_2;
    public GameObject dropPrefab_3;


    private float rate_x;
    private float rate_y;
    public float ScreenX;
    public float ScreenY;

    // ���� �����Ǵ� ����(��)�� �����ϴ� ����
    public float spawnRate;

    // ���� Ÿ�̸Ӹ� �����ϴ� ����
    private float timer;

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
        orgDropPrefeb = dropPrefab_1;

        rate_x = (float)Screen.width / ScreenX;
        rate_y = (float)Screen.height / ScreenY;
       
    }

    void Update()
    {
        // �� �����Ӹ��� Ÿ�̸ӿ� �ð� �߰�
        timer += Time.deltaTime;

        // Ÿ�̸Ӱ� ���� ������ �ʰ����� ��
        if (timer >= spawnRate/10)
        {
            // ���� �����ϴ� �Լ� ȣ��
            SpawnDrop();

            // Ÿ�̸Ӹ� �ʱ�ȭ
            timer = 0;
        }
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