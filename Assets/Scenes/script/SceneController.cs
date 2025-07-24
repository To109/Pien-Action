using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // シングルトン実装
    public static SceneController Instance { get; private set; }

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

    // --- 安全な専用メソッド（コードから呼び出す用）---

    public void LoadTitleScene()
    {
        SoundManager.Instance.PlayBgm(BgmType.Title);
        SceneManager.LoadScene("TitleScene"); // シーン名は実際のファイル名に合わせる
    }

    public void LoadPlayScene()
    {
        // ゲーム開始前のデータセットアップなどはGameManagerが行う
        SceneManager.LoadScene("PlayScene");
    }

    public void LoadRankingScene()
    {
        SceneManager.LoadScene("RankingScene");
    }

    // 他の専用メソッドも必要に応じてここに追加...


    // --- 汎用的なメソッド（UIボタンのOnClick()から呼び出す用）---

    /// <summary>
    /// 引数で指定された名前のシーンをロードします。
    /// </summary>
    /// <param name="sceneName">ロードしたいシーンのファイル名</param>
    public void ChangeScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("シーン名が指定されていません！");
            return;
        }
        SceneManager.LoadScene(sceneName);
    }
}