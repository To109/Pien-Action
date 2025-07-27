using UnityEngine;
using TMPro; // TextMeshProを扱うために必要
using System;  // TimeSpanを扱うために必要

public class PlayUIManager : MonoBehaviour
{
    // シングルトン実装
    public static PlayUIManager Instance { get; private set; }

    [Header("UI要素")]
    [SerializeField]
    private TextMeshProUGUI timeText; // 時間を表示するTextMeshProコンポーネント

    void Awake()
    {
        // シングルトンの設定
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 残り時間の表示を更新するメソッド
    /// </summary>
    /// <param name="remainingSeconds">表示したい残り時間（秒）</param>
    public void UpdateTimeDisplay(float remainingSeconds)
    {
        // 受け取った秒数がマイナスにならないように補正
        if (remainingSeconds < 0)
        {
            remainingSeconds = 0;
        }

        // 秒数を「分：秒」の形式に変換
        TimeSpan timeSpan = TimeSpan.FromSeconds(remainingSeconds);
        string timeString = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        
        // テキストUIに反映
        timeText.text = "TIME: " + timeString;
    }
}