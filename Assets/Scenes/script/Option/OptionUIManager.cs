using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionUIManager : MonoBehaviour // 以前の設計通りUIManagerを継承してもOK
{
    [Header("BGM関連UI")]
    [SerializeField] private Button bgmPlusButton;
    [SerializeField] private Button bgmMinusButton;
    [SerializeField] private Image bgmPienIcon;
    [SerializeField] private Transform bgmSliderStartPoint; // アイコン移動の始点
    [SerializeField] private Transform bgmSliderEndPoint;   // アイコン移動の終点

    [Header("SE関連UI")]
    [SerializeField] private Button sePlusButton;
    [SerializeField] private Button seMinusButton;
    [SerializeField] private Image sePienIcon;
    [SerializeField] private Transform seSliderStartPoint;
    [SerializeField] private Transform seSliderEndPoint;

    [Header("その他UI")]
    [SerializeField] private Button closeButton;

    void Start()
    {
        // --- ボタンがクリックされた時の処理を登録 ---
        bgmPlusButton.onClick.AddListener(OnBgmPlus);
        bgmMinusButton.onClick.AddListener(OnBgmMinus);

        sePlusButton.onClick.AddListener(OnSePlus);
        seMinusButton.onClick.AddListener(OnSeMinus);

        closeButton.onClick.AddListener(OnClose);

        // --- UIの初期表示を更新 ---
        UpdateAllUI();
    }

    // BGM音量を上げる
    private void OnBgmPlus()
    {
        SoundManager.Instance.PlaySe(SeType.UIClick);
        int currentLevel = SoundManager.Instance.BgmVolumeLevel;
        SoundManager.Instance.SetBgmVolume(currentLevel + 1);
        UpdateAllUI(); // UIを更新
    }

    // BGM音量を下げる
    private void OnBgmMinus()
    {
        SoundManager.Instance.PlaySe(SeType.UIClick);
        int currentLevel = SoundManager.Instance.BgmVolumeLevel;
        SoundManager.Instance.SetBgmVolume(currentLevel - 1);
        UpdateAllUI();
    }

    // SE音量を上げる
    private void OnSePlus()
    {
        SoundManager.Instance.PlaySe(SeType.UIClick);
        int currentLevel = SoundManager.Instance.SeVolumeLevel;
        SoundManager.Instance.SetSeVolume(currentLevel + 1);
        UpdateAllUI();
    }

    // SE音量を下げる
    private void OnSeMinus()
    {
        SoundManager.Instance.PlaySe(SeType.UIClick);
        int currentLevel = SoundManager.Instance.SeVolumeLevel;
        SoundManager.Instance.SetSeVolume(currentLevel - 1);
        UpdateAllUI();
    }

    // 閉じるボタン
    private void OnClose()
    {
        SoundManager.Instance.PlaySe(SeType.UIClick);
        // タイトルシーンに戻る（シーン名は実際のファイル名に合わせる）
        SceneController.Instance.ChangeScene("TitleScene");
    }

    /// <summary>
    /// 全てのUI表示を現在の音量設定に合わせて更新する
    /// </summary>
    private void UpdateAllUI()
    {
        // SoundManagerから現在の音量レベルを取得
        int bgmLevel = SoundManager.Instance.BgmVolumeLevel;
        int seLevel = SoundManager.Instance.SeVolumeLevel;

        // ぴえんアイコンの位置を更新
        UpdatePienIconPosition(bgmPienIcon.rectTransform, bgmSliderStartPoint, bgmSliderEndPoint, bgmLevel);
        UpdatePienIconPosition(sePienIcon.rectTransform, seSliderStartPoint, seSliderEndPoint, seLevel);

        // +/-ボタンの有効/無効を切り替え
        bgmMinusButton.interactable = bgmLevel > 0;
        bgmPlusButton.interactable = bgmLevel < 4;
        seMinusButton.interactable = seLevel > 0;
        sePlusButton.interactable = seLevel < 4;
    }

    /// <summary>
    /// ぴえんアイコンの位置を音量レベルに応じて更新する
    /// </summary>
    private void UpdatePienIconPosition(RectTransform icon, Transform start, Transform end, int level)
    {
        // 0-4のレベルを0.0-1.0の割合に変換
        float t = level / 4.0f;
        // 始点と終点の間の位置を線形補間で計算
        icon.position = Vector3.Lerp(start.position, end.position, t);
    }
}