using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SubmitScore : MonoBehaviour
{
    public InputField nameInputField;  // �г����� �Է¹޴� Input Field
    public Button submitButton;  // ��ŷ ��� ��ư
    public GameObject connectErrorPanel;
    public GameObject InputRanking;

    public  void OnEnable()
    {
       
        // ��ư Ŭ�� �̺�Ʈ ����
        submitButton.onClick.AddListener(OnSubmitButtonClicked);
    }

    // ��� ��ư�� ������ �� ����Ǵ� �Լ�
    void OnSubmitButtonClicked()
    {

        string playerName = nameInputField.text;  // �Էµ� �г��� ��������
      
        // �÷��̾ �г����� �Է��ߴ��� Ȯ��
       
        if (!string.IsNullOrEmpty(playerName))
        {
            // ������ ������ �г����� �����ϴ� �ڷ�ƾ ����
            Debug.Log("OnSubmitButtonClicked");
            StartCoroutine(SubmitRank(playerName));
            nameInputField.text = "";
           
            //GameManager.instance.isInputRanking = true;                
        }
        
    }

    // ������ ��ŷ �����͸� POST ��û���� �����ϴ� �Լ�
    IEnumerator SubmitRank(string playerName)
    {
        string guid = PlayerPrefs.GetString("guid");
        string url = "https://secure-taiga-65237-2563e7cea054.herokuapp.com/api/submit-name";  // ������ API ��������Ʈ

        // ������ ������ JSON ������ ����
        RankEntry entry = new RankEntry(guid, playerName);
        string jsonData = JsonUtility.ToJson(entry);

        // POST ��û ����
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // ������ ��û ����
        yield return request.SendWebRequest();

        // ���� ���� ó��
        if (request.result == UnityWebRequest.Result.Success)
        { 
            Debug.Log("ID ��� ����!");
            InputRanking.SetActive(false);
            GameManager.instance.RegisterRankingComplete();
            //GameManager.instance.ShowRanking();
        }
        else
        {
            connectErrorPanel.SetActive(true);
            //Debug.LogError("��ŷ ��� ����: " + request.error);
        }
        request.Dispose();
        
    }
}
