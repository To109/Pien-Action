using UnityEngine;
using UnityEngine.SceneManagement; // SceneManagerを使うために必要

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // ゲーム内データ
    private float remainingTime;
    
    // 他クラスへの参照
    private Player player;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 新しいシーンがロードされた時に自動的に呼ばれるメソッド
    /// </summary>
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// このオブジェクトが破棄される時に呼ばれるメソッド
    /// </summary>
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// シーンがロードされた直後に実行される処理
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // PlaySceneがロードされた場合のみ、ゲームの初期化を行う
        if (scene.name == "PlayScene")
        {
            InitializeGame();
        }
    }

    /// <summary>
    /// ゲームの初期化処理
    /// </summary>
    private void InitializeGame()
    {
        // Playerの参照を取得
        player = FindObjectOfType<Player>();
        if (player == null) return;

        // DifficultyManagerから現在の難易度設定を取得
        DifficultyParameter settings = DifficultyManager.Instance.GetCurrentDifficultyParameters();
        if (settings == null)
        {
            Debug.LogError("難易度設定が見つかりません！");
            return;
        }

        // --- 取得した設定を各所に適用 ---

        // 1. BGMを再生
        SoundManager.Instance.PlayBgm(settings.stageBgm);

        // 2. プレイヤーのライフを設定
        //    (Player.csにSetInitialHealthのようなメソッドが必要)
        player.SetInitialHealth(settings.initialPlayerHealth);

        // 3. 制限時間を設定
        this.remainingTime = settings.initialTimeLimit;
        
        // UIにも反映
        PlayUIManager.Instance.UpdateTimeDisplay(this.remainingTime);
        // PlayUIManager.Instance.UpdateHealthDisplay(settings.initialPlayerHealth, settings.initialPlayerHealth);
    }

    void Update()
    {
        // ゲームがプレイ中でなければ何もしない...
        
        // 制限時間のカウントダウン処理
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            PlayUIManager.Instance.UpdateTimeDisplay(remainingTime);
        }
        else
        {
            // ゲームオーバー処理...
        }
    }
}