using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RankingBoarderManager : MonoBehaviour
{
 
    public RectTransform contentTransform;  // Scroll View의 Content 객체
    public GameObject leaderboardItemPrefab;  // 랭킹 항목에 사용할 프리팹
    public GameObject connectErrorPanel;

    int rank;


 
    private void OnEnable()
    {
        rank = 1;

        StartCoroutine(UpdateScoreAndGetRanking());
       

    }

    IEnumerator UpdateScoreAndGetRanking()
    {
        string url = "https://secure-taiga-65237-2563e7cea054.herokuapp.com/api/update-score";  // score를 update할 url
        string guid = GameManager.instance.guid;
        int score = GameManager.instance.score;
       
        // 서버에 전송할 JSON 데이터 생성
        RankEntry entry = new RankEntry(guid, score);
        string jsonData = JsonUtility.ToJson(entry);

        // POST 요청 생성
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // 서버에 요청 전송
        yield return request.SendWebRequest();

        // 서버 응답 처리
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("score Update 성공!");
            StartCoroutine(GetLeaderboard());
            //GameManager.instance.ShowRanking();
        }
        else
        {
            connectErrorPanel.SetActive(true);
            //Debug.LogError("랭킹 등록 실패: " + request.error);
        }
        request.Dispose();

    }

    // 서버에서 랭킹 데이터를 가져오는 코루틴
    IEnumerator GetLeaderboard()
    {
        string serverUrl = "https://secure-taiga-65237-2563e7cea054.herokuapp.com/api/leaderboard";  // 랭킹 데이터를 가져올 서버의 API URL
        // 서버에 GET 요청을 보냄
        UnityWebRequest request = UnityWebRequest.Get(serverUrl);

        yield return request.SendWebRequest();  // 요청이 완료될 때까지 대기

        // 요청이 성공했을 때 처리
        if (request.result == UnityWebRequest.Result.Success)
        {
            // 서버에서 받은 JSON 데이터를 파싱하여 RankEntry 리스트로 변환
            //Debug.Log(request.downloadHandler.text);
            List<RankEntry> rankEntries = ParseJson(request.downloadHandler.text);


            // 변환된 리스트를 통해 Scroll View에 랭킹 항목을 동적으로 생성
            PopulateLeaderboard(rankEntries);
        }
        else
        {
            // 요청이 실패했을 때 에러 로그 출력
            Debug.LogError("Error fetching leaderboard: " + request.error);
        }
        request.Dispose();
       
    }

    // 서버에서 받은 JSON 데이터를 RankEntry 리스트로 변환하는 함수
    List<RankEntry> ParseJson(string json)
    {
        // JSON 데이터를 파싱하여 RankEntry 배열로 변환
        
        RankEntry[] entries = JsonHelper.FromJson<RankEntry>(json);
        return new List<RankEntry>(entries);  // 배열을 리스트로 변환하여 반환
    }

    // 받아온 랭킹 데이터를 기반으로 Scroll View에 항목을 채우는 함수
    public void PopulateLeaderboard(List<RankEntry> rankEntries)
    {
        // Content 크기를 항목 수에 맞게 조정 (각 항목의 높이는 100)
        // contentTransform.sizeDelta = new Vector2(contentTransform.sizeDelta.x, rankEntries.Count * 100);

        // 이전에 생성된 항목들을 모두 제거
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        // 새로운 랭킹 항목들을 생성
        foreach (RankEntry entry in rankEntries)
        {

            // 프리팹을 인스턴스화하여 새로운 항목을 생성하고 Content 아래에 추가
            GameObject newItem = Instantiate(leaderboardItemPrefab, contentTransform, false);

            // Prefab 내의 텍스트 UI를 업데이트

            Text[] texts = newItem.GetComponentsInChildren<Text>();
            texts[0].text = rank.ToString();
            texts[1].text = entry.playerName;  // 플레이어 이름
            texts[2].text = entry.score.ToString();  // 플레이어 점수

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
