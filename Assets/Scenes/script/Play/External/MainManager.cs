using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    // シングルトンインスタンス（どこからでもアクセス可能）
    public static MainManager Instance { get; private set; }

    // ゲームオーバー時に表示するUI
    [SerializeField, Header("ゲームオーバーUI")]
    private GameObject _gameOverUI;
    // ゲームクリア時に表示するUI
    [SerializeField, Header("ゲームクリアUI")]
    private GameObject _gameClearUI;

    private GameObject _player;　// プレイヤーオブジェクトの参照
    private bool _bShowUI;  // ゲームオーバーとクリアの判定の参照

    // オブジェクト生成時に1回呼ばれる
    private void Awake()
    {
        // シングルトンのインスタンスが未設定なら自身を登録
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // すでに別のインスタンスが存在する場合は自身を破棄（重複防止）
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // シーン内からPlayerを探して参照を保持
        _player = FindObjectOfType<Player>().gameObject;
        _bShowUI = false; // UIはまだ表示していない
        FindObjectOfType<Fade>().FadeStart(_MainStart); // フェード演出が完了したらゲーム開始処理を実行
        _player.GetComponent<Player>().enabled = false; // ゲーム開始前はプレイヤー操作を無効化
        // 敵のスポーンも無効化
        foreach (EnemySpawner enemySpawner in FindObjectsOfType<EnemySpawner>())
        {
            enemySpawner.enabled = false;
        }
    }

    // フェード終了後に呼び出されるゲーム開始処理
    private void _MainStart()
    {
        // プレイヤーの表示を有効にし、操作可能にする
        _player.GetComponent<Renderer>().enabled = true;
        // 敵のスポーンを有効化
        foreach (EnemySpawner enemySpawner in FindObjectsOfType<EnemySpawner>())
        {
            enemySpawner.enabled = true;
        }
    }

    void Update()
    {
        _ShowGameOverUI();　// プレイヤーが消えたらゲームオーバーUI表示
        // _ShowGameClearUI();
    }

    // プレイヤーが消えたらゲームオーバーUIを表示する処理
    private void _ShowGameOverUI()
    {
        // プレイヤーが存在している場合は何もしない
        if (_player != null && _player)
        {
            return;
        }

        // プレイヤーが存在していない場合はゲームオーバーUIを表示
        _gameOverUI.SetActive(true);
        _bShowUI = true;
    }

    // 外部から呼び出してゲームクリアUIを表示する関数
    public void ShowGameClearUI()
    {
        _gameClearUI.SetActive(true);
        _bShowUI = true;
    }

    // 押されたボタンの情報を取得
    public void OnRestart(InputAction.CallbackContext context)
    {
        // ゲームオーバーやクリア出なければ何もしない
        if (!_bShowUI || !context.performed)
        {
            return;
        }
        // 現在のシーンに再読み込みをする
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
