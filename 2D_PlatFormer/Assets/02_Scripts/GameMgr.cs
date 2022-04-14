using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//로비 및 메뉴씬 구현

public enum GameState
{
    GameIng,
    GamePause,
    GameEnd
}

public class GameMgr : MonoBehaviour
{
    //게임 상태
    public GameState gameState;

    //게임 Play UI 연결
    public Text GoldText = null;
    public Text ScoreText = null;
    public Image HpBarImg = null;
    public Button StopBtn = null;
    public Button InvenBtn = null;

    //Inventory UI
    [Header("Inven UI")]
    public GameObject InvenRoot = null;
    bool isInven = false;
    float InvenSpeed = 0;
    //-350 -592, 46 0

    //게임 오버 UI 연결
    [Header("GameOverUI")]
    public GameObject GameOverPanel = null;
    public Text InfoText = null;
    public Button RetryBtn = null;

    //게임 멈춤 UI 연결
    [Header("GamePauseUI")]
    public GameObject GamePausePanel = null;
    public Button Continue_Btn = null;
    public Button Quit_Btn = null;

    [HideInInspector] public int Gold = 0;
    [HideInInspector] public int Score = 0;
    [HideInInspector] public float Hp = 0;

    //게임 BGM
    AudioSource audioSource = null;

    //타이머
    float PlayTimer = 0.0f;

    //드롭 아이템
    public GameObject CoinPrefab = null;

    public static GameMgr Inst;

    void Awake()
    {
        Inst = this;    
    }

    // Start is called before the first frame update
    void Start()
    {
        gameState = GameState.GameIng;
        GameOverPanel.SetActive(false);
        GamePausePanel.SetActive(false);

        this.audioSource = GetComponent<AudioSource>();
        audioSource.Play();

        if (RetryBtn != null)
            RetryBtn.onClick.AddListener(() =>
            {
                //UnityEngine.SceneManagement.SceneManager.LoadScene("InGame");
            });

        if (StopBtn != null)
            StopBtn.onClick.AddListener(GameStop);

        if (Continue_Btn != null)
            Continue_Btn.onClick.AddListener(() =>
            {
                gameState = GameState.GameIng;
                GamePausePanel.SetActive(false);
            });

        if (Quit_Btn != null)
            Quit_Btn.onClick.AddListener(() =>
            {
                GamePausePanel.SetActive(false);
                gameState = GameState.GameEnd;
            });

        if (InvenBtn != null)
            InvenBtn.onClick.AddListener(() =>
            {
                isInven = !isInven;
            });

        InvenSpeed = 2000;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            GameStop();

        if (Input.GetKeyDown(KeyCode.R))
            isInven=!isInven;

        if (gameState == GameState.GameEnd)
        {
            audioSource.Pause();
            Time.timeScale = 0.0f;
            GameOverPanel.SetActive(true);
            InfoText.text = "SOCORE : " + Score.ToString() + "\n\n" +
                "GOLD : " + Gold.ToString() + "\n\n" +
                "PLAY TIME : " + ((int)PlayTimer).ToString() + "s";
        }
        else if (gameState == GameState.GameIng)
        { 
            Time.timeScale = 1;
            PlayTimer += Time.deltaTime;
        }
        else if (gameState == GameState.GamePause)
            Time.timeScale = 0.0f;
            
        if(isInven == true && InvenRoot.transform.position.x <= 365)
        {
            InvenRoot.transform.position = new Vector3(InvenRoot.transform.position.x + InvenSpeed * Time.deltaTime,320, 0);
        }
        else if(isInven == false && InvenRoot.transform.position.x > -10)
        {
            InvenRoot.transform.position = new Vector3(InvenRoot.transform.position.x - InvenSpeed * Time.deltaTime, 320, 0);
        }

    }
    
    public void AddGold(int a_Gold = 10)
    {
        Gold += a_Gold;
        GoldText.text = Gold.ToString();
    }

    public void AddScore(int a_Score = 100)
    {
        Score += a_Score;
        ScoreText.text = "SCORE : " + Score.ToString();
    }

    public void DeHp(float a_Damage = 0.1f)
    {
        if (HpBarImg.fillAmount == 0)
            return;

        HpBarImg.fillAmount -= a_Damage;

        if (HpBarImg.fillAmount <= 0)
            GameOver();
    }

    public void GameStop()
    {
        gameState = GameState.GamePause;
        GamePausePanel.SetActive(true);
    }
    public void GameOver()
    {
        gameState = GameState.GameEnd;
    }

    public void SpawnCoin(Vector3 montr)
    {
        GameObject go = (GameObject)Instantiate(CoinPrefab);
        go.transform.position = montr;
    }
}
