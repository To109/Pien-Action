using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseUIPanel; // インスペクタからポーズ画面のUIパネルを設定

    private bool isPaused = false; // 現在ポーズ中かどうかのフラグ

    void Start()
    {
        // ゲーム開始時は必ず非表示＆時間は通常通りに
        pauseUIPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        // Escキーが押されたら、ポーズ状態を切り替える
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    /// <summary>
    /// ポーズ状態を切り替えるメソッド
    /// </summary>
    public void TogglePause()
    {
        isPaused = !isPaused; // フラグを反転

        if (isPaused)
        {
            // 時間を止めて、ポーズUIを表示する
            Time.timeScale = 0f;
            pauseUIPanel.SetActive(true);
        }
        else
        {
            // 時間を元に戻し、ポーズUIを非表示にする
            Time.timeScale = 1f;
            pauseUIPanel.SetActive(false);
        }
    }

    // --- UIボタンから呼び出すためのメソッド ---

    /// <summary>
    /// ゲームを再開する（Resumeボタン用）
    /// </summary>
    public void ResumeGame()
    {
        // ポーズ状態を解除するだけ
        if (isPaused)
        {
            TogglePause();
        }
    }

    /// <summary>
    /// 現在のシーンをリスタートする（Restartボタン用）
    /// </summary>
    public void RestartScene()
    {
        // 時間を元に戻してからシーンをリロードすることが重要
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// タイトルシーンに戻る（Titleボタン用）
    /// </summary>
    public void GoToTitleScene()
    {
        // 時間を元に戻してからシーンを遷移する
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScene"); // "TitleScene"は実際のシーン名に合わせる
    }
}