using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    // public 
    public static GameManager instance;
    public bool isGameOver = false;
    public bool levelUpFlag = false;
    public bool isInputRanking = false;
    public Text scoreText;
    public GameObject InGame;
    public GameObject gameOverUI;
    public GameObject intro;
    public GameObject menu;
    public GameObject pauseButton;
    public GameObject InputRanking;
    public GameObject OutputRanking;
    public GameObject Sky;
    public GameObject connectErrorPanel;

    public GameObject NowGage;
    public GameObject FullGage;
    public GameObject LifeHeart;
   



    public GameObject playerPrefab;
    //public Text rankingScore;
    public int score = 0;

    private float rate_x;
    private float rate_y;
    public float ScreenX;
    public float ScreenY;

    private SpriteRenderer SkyRender;
    // private
    int level = 1;
    int addScore = 1;
    int levelUpScore = 100;
    float levelUpRate = 0.2f;
    bool isStart = false;

    float v_fullGage = 0.25f;
    float v_addGage = 0f;
    public bool isFullGage = false;
    public string guid;


    // Start is called before the first frame update

    void Awake()
    {
        
        if (instance == null) { 
            instance = this;
        }
        else{
            Debug.Log("씬 두개 이상의 instance 가 존재합니다");
            Destroy(gameObject);
        }
    }

    void Start()
    {

        //intro.SetActive(true);
        guid = GetOrCreateDeviceID();
        StartCoroutine(GetNameByGuid());
        //GetOrCreateDeviceID();
        intro.SetActive(true);
        InGame.SetActive(false);
        LifeHeart.SetActive(false);
        FullGage.SetActive(false);
        
        rate_x = (float)Screen.width / ScreenX;
        rate_y = (float)Screen.height / ScreenY;
        Debug.Log("rate_y : " + rate_x + " rate_y : " + rate_y);

        SkyRender = Sky.GetComponent<SpriteRenderer>();
        Debug.Log("baseRender.bounds.size.y : " + SkyRender.bounds.size.y + " Screen.height : " + Screen.height);
        //  Instantiate(playerPrefab);
       
        v_addGage = v_fullGage * 0.001f;
        NowGage.transform.localScale = new Vector3(0, NowGage.transform.localScale.y, 0);
    }


    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        if (!InputRanking.activeSelf && Input.GetMouseButtonDown(0) && !isStart)
        {
            intro.SetActive(false);
            InGame.SetActive(true);
            pauseButton.SetActive(true);
            scoreText.gameObject.SetActive(true);


            GameObject player = Instantiate(playerPrefab, new Vector3(0, -3.95f * rate_y, 0), Quaternion.identity);
            player.transform.localScale = new Vector3(player.transform.localScale.x * rate_x, player.transform.localScale.y * rate_y, 1);
            isStart = true;
        }

        if (level <= (score / levelUpScore) && level < 10)
        {
            level = (score / levelUpScore) + 1;
            LevelUP();
        }

        scoreText.text = "Score : " + score;

        //Debug.Log("TIme : " + Time.realtimeSinceStartup);


#elif UNITY_ANDROID || UNITY_IOS
        if (!InputRanking.activeSelf && !isStart && Input.touchCount > 0)
        {
            intro.SetActive(false);
            InGame.SetActive(true);
            pauseButton.SetActive(true);
            scoreText.gameObject.SetActive(true);


            GameObject player = Instantiate(playerPrefab, new Vector3(0, -3.95f * rate_y, 0), Quaternion.identity);
            player.transform.localScale = new Vector3(player.transform.localScale.x * rate_x, player.transform.localScale.y * rate_y, 1);
            isStart = true;
        }

        if (level <= (score / levelUpScore) && level < 10)
        {
            level = (score / levelUpScore) + 1;
            LevelUP();
        }

        scoreText.text = "Score : " + score;
#endif
    }
    public void AddScore()
    {
        if (!isGameOver)
        {
            score += addScore;
            if (v_fullGage > NowGage.transform.localScale.x)
            {
                NowGage.transform.localScale = new Vector3(NowGage.transform.localScale.x + v_addGage, NowGage.transform.localScale.y, 0);
            }
            if(v_fullGage <= NowGage.transform.localScale.x && isFullGage == false)
            {
                FullGage.SetActive(true);
                NowGage.SetActive(false);
                LifeHeart.SetActive(true);
                isFullGage = true;
            }
        }
        
    }
    public void GameOver()
    {
       
        isGameOver = true; 
       // gameOverUI.SetActive(true);
        menu.SetActive(false);
        ShowRanking();
        //RegisterRanking();

    }
    public void RestartGame()
    {


        destroyAllEntity();
        destroyAllDrops();
        OutputRanking.SetActive(false);


        // level 초기화
        
        level = 1;
        addScore = 1;
        score = 0;
        DropSpawner.instance.spawnRate = 3.0f;
        isGameOver = false;
        
        isFullGage = false;
        NowGage.transform.localScale = new Vector3(0, NowGage.transform.localScale.y, 0);
        LifeHeart.SetActive(false);
        FullGage.SetActive(false);
        NowGage.SetActive(true);

        GameObject player = Instantiate(playerPrefab, new Vector3(0, -3.95f * rate_y, 0), Quaternion.identity);
        player.transform.localScale = new Vector3(player.transform.localScale.x * rate_x, player.transform.localScale.y * rate_y, 1);
        pauseButton.SetActive(true); 
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    public void PauseGame()
    {
        menu.SetActive(true);
        Time.timeScale = 0f;  // 시간 멈춤

    }
    public void ResumeGame()
    {
        menu.SetActive(false);
        Time.timeScale = 1f;  // 시간 다시 흐름

    }

    public void  RegisterRanking()
    {
        pauseButton.SetActive(false);
        gameOverUI.SetActive(false);
        InputRanking.SetActive(true);

    }

    public void ShowRanking()
    {
        OutputRanking.SetActive(true);
    }

    public void LevelUP()
    {
        
        
        Debug.Log("Level UP!");
        addScore++;
        DropSpawner.instance.spawnRate -= levelUpRate;
        //DropSpawner.instance.ChangeDrop(level);

    }
    public void CloseConnectErrorPanel()
    {
        connectErrorPanel.SetActive(false);
    }

    public void CloseInputRankingPanel()
    {
        InputRanking.SetActive(false);
        ShowRanking();
    }
    public  void destroyAllEntity()
    {
        GameObject[] entities = GameObject.FindGameObjectsWithTag("Entity");
        foreach (GameObject entity in entities)
        {
            Destroy(entity.gameObject);
        }
    }
    public  void destroyAllDrops()
    {
        //Drop drop;

        GameObject[] drops = GameObject.FindGameObjectsWithTag("Drop");


        foreach (GameObject drop in drops)
        {
            //drop.destroyDrops();
            Drop v_drop = drop.GetComponent<Drop>();
            v_drop.destroyDrops();
            //Destroy(drop.gameObject);
        }
       
    }

    public string   GetOrCreateDeviceID()
    {

        if (PlayerPrefs.HasKey("guid"))
        {
            Debug.Log("exist guid!");
            return PlayerPrefs.GetString("guid");
        }
        else
        {
            string newGuid = System.Guid.NewGuid().ToString();

            Debug.Log("create newGuid : " + newGuid);
            PlayerPrefs.SetString("guid", newGuid);
            PlayerPrefs.Save();

            RegisterRanking();

            return newGuid;
        }
    }

    IEnumerator GetNameByGuid()
    {
        string url = "https://secure-taiga-65237-2563e7cea054.herokuapp.com/api/get-name";  // name를 select할 url
        string guid = GameManager.instance.guid;
       

        // 서버에 전송할 JSON 데이터 생성
        RankEntry entry = new RankEntry(guid);
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
            if (request.downloadHandler.text.Length > 0)
            {
                Debug.Log("get Name 성공! Name : " + request.downloadHandler.text);
            }
            else
            {
                Debug.Log("get Name 실패!");
                RegisterRanking();
            }

        }
        else
        {
            connectErrorPanel.SetActive(true);
            //Debug.LogError("랭킹 등록 실패: " + request.error);
        }
        request.Dispose();

    }

    List<RankEntry> ParseJson(string json)
    {
        // JSON 데이터를 파싱하여 RankEntry 배열로 변환

        RankEntry[] entries = JsonHelper.FromJson<RankEntry>(json);
        return new List<RankEntry>(entries);  // 배열을 리스트로 변환하여 반환
    }


}
