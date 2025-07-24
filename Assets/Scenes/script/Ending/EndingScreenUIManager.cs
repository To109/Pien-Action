using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class EndingScreenUIManager : MonoBehaviour
{
    // インスペクタから設定するUIパーツ
    [Header("エンディングCG")]
    public Image a_image;
    public Image b_image;
    public Image c_image;

    [Header("テキスト関連")]
    public TextMeshProUGUI endTextDisplay; // テキスト表示エリア
    public GameObject nextPageIndicator; // ▶ マークのUIオブジェクト

    [Header("ボタン")]
    public Button endingSkipButton;

    // --- 内部で使う変数 ---
    private EndingContent currentEndingContent; // 表示するエンディングのデータ
    private bool isRankedIn; // ランキング登録対象か
    private Coroutine typewriterCoroutine; // テキスト表示コルーチンの参照

    void Start()
    {
        // スキップボタンが押されたらOnSkipButtonClickedメソッドを呼ぶ
        endingSkipButton.onClick.AddListener(OnSkipButtonClicked);

        // デバッグ用!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!に、Aエンドを開始してみる
        // 修正前: StartEnding(EndingType.A, true);
        Setup(EndingType.A, true); // 修正後: メソッド名を「Setup」に変更
    }

    // SceneControllerから呼び出されるエントリーポイント
    public void Setup(EndingType type, bool rankedIn)
    {
        this.isRankedIn = rankedIn; // 次のシーン遷移のために保持

        // ① 対応するエンディングのデータをEndingManagerから取得
        currentEndingContent = EndingManager.Instance.GetEndingContent(type);

        // ② CGをセットアップ
        a_image.gameObject.SetActive(type == EndingType.A);
        b_image.gameObject.SetActive(type == EndingType.B);
        c_image.gameObject.SetActive(type == EndingType.C);

        // a_image.sprite = currentEndingContent.endingCg; // より汎用的な実装

        // ③ ストーリー再生を開始
        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
        }
        typewriterCoroutine = StartCoroutine(PlayStory());
    }

    private IEnumerator PlayStory()
    {
        // 全文を改ページ文字（\p）で分割する
        string[] storyPages = currentEndingContent.storyText.Split(new[] { "\\p" }, System.StringSplitOptions.None);

        foreach (string page in storyPages)
        {
            // 1ページ分のタイプライター表示が終わるまで待つ
            yield return StartCoroutine(TypewriterEffect(page));

            // ▶ マークを表示してクリックを待つ
            nextPageIndicator.SetActive(true);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0)); // 🖱️ クリック判定はここ！
            nextPageIndicator.SetActive(false);
        }

        // 全てのテキストが終わったら次のシーンへ
        FinishEnding();
    }

    private IEnumerator TypewriterEffect(string textToShow)
    {
        endTextDisplay.text = ""; // テキストをリセット
        foreach (char c in textToShow)
        {
            endTextDisplay.text += c;
            yield return new WaitForSeconds(0.05f); // 1文字あたりの表示ウェイト
        }
    }

    public void OnSkipButtonClicked()
    {
        // 再生中のコルーチンを停止
        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
        }
        // 即座に終了処理へ
        FinishEnding();
    }

    private void FinishEnding()
    {
        // 保持しておいたランクイン判定結果に応じて次のシーンへ
        if (isRankedIn)
        {
            // ランキング登録と表示は同じシーンで行う設計なので、同じメソッドを呼ぶ
            SceneController.Instance.LoadTitleScene();
        }
        else
        {
            SceneController.Instance.LoadTitleScene();
        }
    }
}