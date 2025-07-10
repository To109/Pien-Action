using UnityEngine;
using UnityEngine.SceneManagement; // シーン操作に必要

public class EndingSceneController : MonoBehaviour
{
    // シングルトン実装
    public static EndingSceneController Instance { get; private set; }

    void Awake()
    {
        // シングルトンの設定
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

    // ランキング登録画面へ遷移するメソッド
    public void LoadRankingRegisterScene()
    {
        // "RankingScene"という名前のシーンをロードすることを想定
        // 実際のシーン名に合わせてください
        SceneManager.LoadScene("RankingScene");
    }

    // ランキング表示画面へ遷移するメソッド
    public void LoadRankingDisplayScene()
    {
        // こちらも"RankingScene"をロードし、
        // RankingUIManagerが表示を制御することを想定
        SceneManager.LoadScene("RankingScene");
    }
}