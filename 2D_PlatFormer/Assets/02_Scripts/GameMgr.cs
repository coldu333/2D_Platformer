using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UI와 스크립트 연결
//데미지
//몬스터
//타일맵
//게임오버씬
//게임 컨셉 디벨롭
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

    //게임 오버 UI 연결
    [Header("GameOverUI")]
    public GameObject GameOverPanel = null;
    public Text InfoText = null;
    public Button RetryBtn = null;

    //게임 멈춤 UI 연결
    [Header("GamePauseUI")]
    public GameObject GamePausePanel = null;
    public Button Continue_Btn = null;

    [HideInInspector] public int Gold = 0;
    [HideInInspector] public int Score = 0;
    [HideInInspector] public float Hp = 0;

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

        if (RetryBtn != null)
            RetryBtn.onClick.AddListener(() =>
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("InGame");
            });

        if (StopBtn != null)
            StopBtn.onClick.AddListener(() =>
            {
                gameState = GameState.GamePause;
                GamePausePanel.SetActive(true);
            });

        if (Continue_Btn != null)
            Continue_Btn.onClick.AddListener(() =>
            {
                gameState = GameState.GameIng;
                GamePausePanel.SetActive(false);
            });
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.GameEnd)
        {
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
            gameState = GameState.GameEnd;
    }

    public void SpawnCoin(Vector3 montr)
    {
        GameObject go = (GameObject)Instantiate(CoinPrefab);
        go.transform.position = montr;
    }
}
