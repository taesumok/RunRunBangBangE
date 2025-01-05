using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SubmitScore : MonoBehaviour
{
    public InputField nameInputField;  // 닉네임을 입력받는 Input Field
    public Button submitButton;  // 랭킹 등록 버튼
    public GameObject connectErrorPanel;
    public GameObject InputRanking;

    public  void OnEnable()
    {
       
        // 버튼 클릭 이벤트 설정
        submitButton.onClick.AddListener(OnSubmitButtonClicked);
    }

    // 등록 버튼을 눌렀을 때 실행되는 함수
    void OnSubmitButtonClicked()
    {

        string playerName = nameInputField.text;  // 입력된 닉네임 가져오기
      
        // 플레이어가 닉네임을 입력했는지 확인
       
        if (!string.IsNullOrEmpty(playerName))
        {
            // 서버에 점수와 닉네임을 전송하는 코루틴 실행
            Debug.Log("OnSubmitButtonClicked");
            StartCoroutine(SubmitRank(playerName));
            nameInputField.text = "";
           
            //GameManager.instance.isInputRanking = true;                
        }
        
    }

    // 서버에 랭킹 데이터를 POST 요청으로 전송하는 함수
    IEnumerator SubmitRank(string playerName)
    {
        string guid = PlayerPrefs.GetString("guid");
        string url = "https://secure-taiga-65237-2563e7cea054.herokuapp.com/api/submit-name";  // 서버의 API 엔드포인트

        // 서버에 전송할 JSON 데이터 생성
        RankEntry entry = new RankEntry(guid, playerName);
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
            Debug.Log("ID 등록 성공!");
            InputRanking.SetActive(false);
            //GameManager.instance.ShowRanking();
        }
        else
        {
            connectErrorPanel.SetActive(true);
            //Debug.LogError("랭킹 등록 실패: " + request.error);
        }
        request.Dispose();
        
    }
}
