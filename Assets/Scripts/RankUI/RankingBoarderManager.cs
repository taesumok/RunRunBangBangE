using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RankingBoarderManager : MonoBehaviour
{
 
    public RectTransform contentTransform;  // Scroll View�� Content ��ü
    public GameObject leaderboardItemPrefab;  // ��ŷ �׸� ����� ������
    public GameObject connectErrorPanel;

    int rank;


 
    private void OnEnable()
    {
        rank = 1;

        StartCoroutine(UpdateScoreAndGetRanking());
       

    }

    IEnumerator UpdateScoreAndGetRanking()
    {
        string url = "https://secure-taiga-65237-2563e7cea054.herokuapp.com/api/update-score";  // score�� update�� url
        string guid = GameManager.instance.guid;
        int score = GameManager.instance.score;
       
        // ������ ������ JSON ������ ����
        RankEntry entry = new RankEntry(guid, score);
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
            Debug.Log("score Update ����!");
            StartCoroutine(GetLeaderboard());
            //GameManager.instance.ShowRanking();
        }
        else
        {
            connectErrorPanel.SetActive(true);
            //Debug.LogError("��ŷ ��� ����: " + request.error);
        }
        request.Dispose();

    }

    // �������� ��ŷ �����͸� �������� �ڷ�ƾ
    IEnumerator GetLeaderboard()
    {
        string serverUrl = "https://secure-taiga-65237-2563e7cea054.herokuapp.com/api/leaderboard";  // ��ŷ �����͸� ������ ������ API URL
        // ������ GET ��û�� ����
        UnityWebRequest request = UnityWebRequest.Get(serverUrl);

        yield return request.SendWebRequest();  // ��û�� �Ϸ�� ������ ���

        // ��û�� �������� �� ó��
        if (request.result == UnityWebRequest.Result.Success)
        {
            // �������� ���� JSON �����͸� �Ľ��Ͽ� RankEntry ����Ʈ�� ��ȯ
            //Debug.Log(request.downloadHandler.text);
            List<RankEntry> rankEntries = ParseJson(request.downloadHandler.text);


            // ��ȯ�� ����Ʈ�� ���� Scroll View�� ��ŷ �׸��� �������� ����
            PopulateLeaderboard(rankEntries);
        }
        else
        {
            // ��û�� �������� �� ���� �α� ���
            Debug.LogError("Error fetching leaderboard: " + request.error);
        }
        request.Dispose();
       
    }

    // �������� ���� JSON �����͸� RankEntry ����Ʈ�� ��ȯ�ϴ� �Լ�
    List<RankEntry> ParseJson(string json)
    {
        // JSON �����͸� �Ľ��Ͽ� RankEntry �迭�� ��ȯ
        
        RankEntry[] entries = JsonHelper.FromJson<RankEntry>(json);
        return new List<RankEntry>(entries);  // �迭�� ����Ʈ�� ��ȯ�Ͽ� ��ȯ
    }

    // �޾ƿ� ��ŷ �����͸� ������� Scroll View�� �׸��� ä��� �Լ�
    public void PopulateLeaderboard(List<RankEntry> rankEntries)
    {
        // Content ũ�⸦ �׸� ���� �°� ���� (�� �׸��� ���̴� 100)
        // contentTransform.sizeDelta = new Vector2(contentTransform.sizeDelta.x, rankEntries.Count * 100);

        // ������ ������ �׸���� ��� ����
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        // ���ο� ��ŷ �׸���� ����
        foreach (RankEntry entry in rankEntries)
        {

            // �������� �ν��Ͻ�ȭ�Ͽ� ���ο� �׸��� �����ϰ� Content �Ʒ��� �߰�
            GameObject newItem = Instantiate(leaderboardItemPrefab, contentTransform, false);

            // Prefab ���� �ؽ�Ʈ UI�� ������Ʈ

            Text[] texts = newItem.GetComponentsInChildren<Text>();
            texts[0].text = rank.ToString();
            texts[1].text = entry.playerName;  // �÷��̾� �̸�
            texts[2].text = entry.score.ToString();  // �÷��̾� ����

            rank++;
        }

      




    }
    void OnDisable()
    {
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }
 
    }
}

[System.Serializable]
public class RankEntry
{
    public string guid;
    public int Rank;
    public string playerName;
    public int score;


    public RankEntry(string guid)
    {
        this.guid = guid;
    }
    public RankEntry(string guid, string playerName)
    {
        this.guid = guid;
        this.playerName = playerName;
    }

    public RankEntry(string guid, int score)
    {
        this.guid = guid;
        this.score = score;
    }

}
