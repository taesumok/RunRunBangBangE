using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
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
    public bool isShield = false;
    public Text scoreText;
    public GameObject InGame;
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

    public GameObject startButton;
    public GameObject startButton2;

    public GameObject restartButton;
    public GameObject restartButton2;
    public GameObject showRankingButton;
    public GameObject namePannel;
    public GameObject rankingCloseButtons;
    
    public Text lifeGageText;

    public Text showLevelText; 




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
    public int addScore = 1;
    public int levelUpScore;

    public int addLevelUpScore = 200;
    float levelUpRate = 0.4f;

    public bool checkLevelUP = false;
    public bool isStart = false;

    float v_fullGage = 0.25f;
    float v_addGage = 0f;
    public bool isFullGage = false;
    public string guid;

    public float colorChangeSpeed;

    AudioSource audioSource;
    public AudioClip bgm_clip;
    public AudioClip die_cilp;
    public AudioClip getLife_cilp;

    public bool audioOn = true;

    bool isFirstLevel = true;


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

        if(isStart){
            if(isFirstLevel){
                StartCoroutine(WaitForNextLevel());
                isFirstLevel = false;
            }
            if (checkLevelUP)
            {
                
                if ( score >= levelUpScore )
                {
                    levelUpScore += addLevelUpScore;
                    addLevelUpScore+=100;
                    level++;
                    if(level > 5){
                        levelUpRate*=0.5f;
                    }

                    DropTimerInit();

                    DropSpawner.instance.spawnRate -= levelUpRate;
                    DropSpawner.instance.spaceSpawnRate -= levelUpRate*50;
                    DropSpawner.instance.spaceSpawnRate -= levelUpRate*60;

                    destroyAllDrops();
                    destroyAllSpaceship();
                    destroyAllItem();
                    StartCoroutine(WaitForNextLevel());

                }
            }
            //scoreText.text = "Score : " + score;
            scoreText.text = "Lv" + level + "\n" + score.ToString() + "/" + levelUpScore.ToString();
        }
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
            checkLevelUP = true;
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
        isStart = false;
        //RegisterRanking();

    }
    public void StartGame()
    {
        if (intro.activeSelf && !InputRanking.activeSelf)
        {
            if (audioOn == true)
            {
                audioSource.Play();
            }
            intro.SetActive(false);
            InGame.SetActive(true);
            pauseButton.SetActive(true);
            lifeGageText.gameObject.SetActive(true);



            GameObject player = Instantiate(playerPrefab, new Vector3(0, -3.95f * rate_y, 0), Quaternion.identity);
            player.transform.localScale = new Vector3(player.transform.localScale.x * rate_x, player.transform.localScale.y * rate_y, 1);
            isStart = true;
        }
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
        destroyAllSpaceship();
        destroyAllItem();
        DropTimerInit();
        OutputRanking.SetActive(false);


        // level ????
        
        level = 1;
        addScore = 1;
        score = 0;
        levelUpScore = 100;
        addLevelUpScore = 200;
        DropSpawner.instance.spawnCount = 0;
        DropSpawner.instance.spawnRate = 3f;
        DropSpawner.instance.spaceSpawnRate = 150f;
        DropSpawner.instance.itemSpawnRate = 180f;
        levelUpRate = 0.4f;
       
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
        isStart = true;
        isFirstLevel = true;
        SkyManager.instance.spriteRenderer.color = Color.white;
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
        namePannel.SetActive(false);
        startButton.SetActive(false);
        startButton2.SetActive(false);
        showRankingButton.SetActive(false);

        InputRanking.SetActive(true);

    }
    public void RegisterRankingComplete()
    {
        intro.SetActive(true);
        namePannel.SetActive(true);
        startButton.SetActive(true);
        startButton2.SetActive(true);
        showRankingButton.SetActive(true);

    }

    public void ShowRanking()
    {
        OutputRanking.SetActive(true);
        restartButton.SetActive(true);
        restartButton2.SetActive(true);
        rankingCloseButtons.SetActive(false);
    }

    public void ShowRankingIntro()
    {
        namePannel.SetActive(false);
        startButton.SetActive(false);
        startButton2.SetActive(false);
        showRankingButton.SetActive(false);

        OutputRanking.SetActive(true);

        restartButton.SetActive(false);
        restartButton2.SetActive(false);
        rankingCloseButtons.SetActive(true);

    }

    public void CloseShowRankingIntro()
    {
        OutputRanking.SetActive(false);
        namePannel.SetActive(true);
        startButton.SetActive(true);
        startButton2.SetActive(true);
        showRankingButton.SetActive(true);
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

    public  void destroyAllSpaceship()
    {
        GameObject[] entities = GameObject.FindGameObjectsWithTag("SpaceShip");
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

     public  void destroyAllItem()
    {
        GameObject[] entities = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject entity in entities)
        {
            Destroy(entity.gameObject);
        }

        entities = GameObject.FindGameObjectsWithTag("ItemShield");
        foreach (GameObject entity in entities)
        {
            Destroy(entity.gameObject);
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
    public void SetDoubleScore()
    {
        StartCoroutine(C_SetDoubleScore());
    }
    IEnumerator C_SetDoubleScore()
    {
        addScore *= 2;
        yield return new WaitForSeconds(5f);
        if(addScore >= 2){
            addScore /= 2;
        }
        else{
            addScore = 1;
        }
    }

    public void SetShield()
    {
        StartCoroutine(C_SetShield());
    }
    IEnumerator C_SetShield()
    {
        isShield = true;
        yield return new WaitForSeconds(5f);
        isShield = false;
    }

    IEnumerator GetNameByGuid()
    {
        string url = "https://secure-taiga-65237-2563e7cea054.herokuapp.com/api/get-name";  // name?? select?? url
       

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
                RegisterRankingComplete();
            }
            else
            {
                Debug.Log("get Name ????!");
                RegisterRanking();
            }
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



    IEnumerator WaitForNextLevel(){
        Debug.Log("WaitForNextLevel()");
        showLevelText.color = new Color(showLevelText.color.r,showLevelText.color.g,showLevelText.color.b, 1f);
        showLevelText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(false);

        DropSpawner.instance.doSpawn = false;   
        checkLevelUP = false;
        yield return StartCoroutine(ShowLevel());
        //checkLevelUP = true;
        DropSpawner.instance.doSpawn = true;   

        SkyManager.instance.spriteRenderer.color = new Color(SkyManager.instance.spriteRenderer.color.r, SkyManager.instance.spriteRenderer.color.g-0.05f, SkyManager.instance.spriteRenderer.color.b-0.05f);
        Debug.Log("WaitForNextLevel() end");
    }
    
    IEnumerator ShowLevel(){
        showLevelText.text = "LEVEL " + level;
    
        Color color = showLevelText.color;
        //Debug.Log("color.a : " + color.a);
        while (color.a > 0) // ���� �ݺ����� ��ġ �ؽ�Ʈ ������ 
        {
            
            //Debug.Log("color.a : " + color.a);
            color.a -= 0.05f;
            showLevelText.color = color;
            yield return new WaitForSeconds(0.15f);

        }
        showLevelText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);
    
    }

    void DropTimerInit()
    {
        DropSpawner.instance.timer = 0;
        DropSpawner.instance.itemTimer = 0;
        DropSpawner.instance.shipTimer = 0;
    }

}
