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

    public GameObject finger;
   



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

    public float colorChangeSpeed = 0f;

    AudioSource audioSource;
    public AudioClip bgm_clip;
    public AudioClip die_cilp;
    public AudioClip getLife_cilp;

    bool audioOn = true;


    public Text auidioText;

    

    




    // Start is called before the first frame update

    void Awake()
    {
        
        if (instance == null) { 
            instance = this;
        }
        else{
            Debug.Log("?? ??? ????? instance ?? ????????");
            Destroy(gameObject);
        }
    }

    void Start()
    {

        //intro.SetActive(true);
        guid = GetOrCreateDeviceID();
        StartCoroutine(GetNameByGuid());
        //GetOrCreateDeviceID();
        //intro.SetActive(true); => GetNameByGuid 내로 이동
        InGame.SetActive(false);
        LifeHeart.SetActive(false);
        FullGage.SetActive(false);
        
        rate_x = (float)Screen.width / ScreenX;
        rate_y = (float)Screen.height / ScreenY;
        Debug.Log("rate_y : " + rate_x + " rate_y : " + rate_y);

        
        //SkyRender = Sky.GetComponent<SpriteRenderer>();
        //Debug.Log("baseRender.bounds.size.y : " + SkyRender.bounds.size.y + " Screen.height : " + Screen.height);
        //  Instantiate(playerPrefab);
       
        v_addGage = v_fullGage * 0.002f;
        
        NowGage.transform.localScale = new Vector3(0, NowGage.transform.localScale.y, 0);

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = bgm_clip;
        
    }


    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        if (intro.activeSelf && !InputRanking.activeSelf && Input.GetMouseButtonDown(0) && !isStart)
        {
            if(audioOn == true){
                audioSource.Play();
            }
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

        //scoreText.text = "Score : " + score;
        scoreText.text = score.ToString();

        //Debug.Log("TIme : " + Time.realtimeSinceStartup);


#elif UNITY_ANDROID || UNITY_IOS
        if (intro.activeSelf && !InputRanking.activeSelf && !isStart && Input.touchCount > 0)
        {
            if(audioOn == true){
                audioSource.Play();
            }
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

        //scoreText.text = "Score : " + score;
        scoreText.text = score.ToString();
#endif
    }
    public void SoundOnOFF()
    {
        if(audioOn == true){
            audioSource.Stop();
            auidioText.text = "Sound OFF";
            auidioText.color = new Color(0.5f,0.5f,0.5f);
            audioOn = false;
        }
        else{
            audioSource.Play();
            auidioText.text = "Sound ON";
            auidioText.color = new Color(0,0,0);
            audioOn = true;
        }

    }
    public void AddScore()
    {
        if (!isGameOver)
        {
            score += addScore;
            //colorChangeSpeed += 0.001f; 
            if (v_fullGage > NowGage.transform.localScale.x)
            {
                NowGage.transform.localScale = new Vector3(NowGage.transform.localScale.x + v_addGage, NowGage.transform.localScale.y, 0);
            }
            if(v_fullGage <= NowGage.transform.localScale.x && isFullGage == false)
            {
                GetLife();
            }


        }
        
    }
    public void GetLife()
    {
        FullGage.SetActive(true);
        NowGage.SetActive(false);
        LifeHeart.SetActive(true);
        isFullGage = true;
        if(audioOn == true){
            audioSource.PlayOneShot(getLife_cilp);
        }
    }
    public void GameOver()
    {
        if(audioOn == true){
            audioSource.Stop();
            audioSource.clip = die_cilp;
            audioSource.loop = false;
            audioSource.Play();
        }

        isGameOver = true; 
       // gameOverUI.SetActive(true);
        pauseButton.SetActive(false);
        menu.SetActive(false);
        ShowRanking();
        //RegisterRanking();

    }
    public void RestartGame()
    {
        if(audioOn == true){
            audioSource.Stop();
            audioSource.clip = bgm_clip;
            audioSource.loop = true;
            audioSource.Play();
        }

        destroyAllEntity();
        destroyAllDrops();
        OutputRanking.SetActive(false);


        // level ????
        
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
        finger.SetActive(true);

        GameObject player = Instantiate(playerPrefab, new Vector3(0, -3.95f * rate_y, 0), Quaternion.identity);
        player.transform.localScale = new Vector3(player.transform.localScale.x * rate_x, player.transform.localScale.y * rate_y, 1);
        pauseButton.SetActive(true); 
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    public void PauseGame()
    {
        menu.SetActive(true);
        Time.timeScale = 0f;  // ?��? ????

    }
    public void ResumeGame()
    {
        menu.SetActive(false);
        Time.timeScale = 1f;  // ?��? ??? ??

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
        //addScore++;
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
        string url = "https://secure-taiga-65237-2563e7cea054.herokuapp.com/api/get-name";  // name?? select?? url
        string guid = GameManager.instance.guid;
       

        // ?????? ?????? JSON ?????? ????
        RankEntry entry = new RankEntry(guid);
        string jsonData = JsonUtility.ToJson(entry);

        // POST ??? ????
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // ?????? ??? ????
        yield return request.SendWebRequest();

        // ???? ???? ???
        if (request.result == UnityWebRequest.Result.Success)
        {
            if (request.downloadHandler.text.Length > 0)
            {
                Debug.Log("get Name ????! Name : " + request.downloadHandler.text);
            }
            else
            {
                Debug.Log("get Name ????!");
                RegisterRanking();
            }
            intro.SetActive(true);

        }
        else
        {
            connectErrorPanel.SetActive(true);
            //Debug.LogError("??? ??? ????: " + request.error);��
        }
        request.Dispose();

    }

    List<RankEntry> ParseJson(string json)
    {
        // JSON ??????? ?????? RankEntry ?��?? ???

        RankEntry[] entries = JsonHelper.FromJson<RankEntry>(json);
        return new List<RankEntry>(entries);  // ?��?? ??????? ?????? ???
    }


}
