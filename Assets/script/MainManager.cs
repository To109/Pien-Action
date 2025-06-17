using UnityEngine;

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
        _player = FindAnyObjectByType<Player>().gameObject;
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
    }

    // 外部から呼び出してゲームクリアUIを表示する関数
    public void ShowGameClearUI()
    {
        _gameClearUI.SetActive(true);
    }
}
