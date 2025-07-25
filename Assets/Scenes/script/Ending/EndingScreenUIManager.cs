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
    // private bool isRankedIn; // ★★ ランキング登録対象か → 不要なので削除 ★★
    private Coroutine typewriterCoroutine; // テキスト表示コルーチンの参照

    void Start()
    {
        // スキップボタンが押されたらOnSkipButtonClickedメソッドを呼ぶ
        endingSkipButton.onClick.AddListener(OnSkipButtonClicked);

        // デバッグ用に、Bエンドを開始してみる
        Setup(EndingType.B); // ★★ 引数を修正 ★★
    }

    // SceneControllerから呼び出されるエントリーポイント
    public void Setup(EndingType type) // ★★ 引数を修正 ★★
    {
        // this.isRankedIn = rankedIn; // ★★ 不要なので削除 ★★

        // ★★ ① 対応するエンディングのBGMを再生する処理を追加 ★★
        PlayEndingBgm(type);

        // ② 対応するエンディングのデータをEndingManagerから取得
        currentEndingContent = EndingManager.Instance.GetEndingContent(type);

        // ③ CGをセットアップ
        a_image.gameObject.SetActive(type == EndingType.A);
        b_image.gameObject.SetActive(type == EndingType.B);
        c_image.gameObject.SetActive(type == EndingType.C);

        // ④ ストーリー再生を開始
        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
        }
        typewriterCoroutine = StartCoroutine(PlayStory());
    }

    // ★★ BGM再生用のメソッドを新たに追加 ★★
    private void PlayEndingBgm(EndingType type)
    {
        BgmType bgmToPlay = BgmType.None; // デフォルトは音なし
        switch (type)
        {
            case EndingType.A:
                bgmToPlay = BgmType.END_A;
                break;
            case EndingType.B:
                bgmToPlay = BgmType.END_B;
                break;
            case EndingType.C:
                bgmToPlay = BgmType.END_C;
                break;
        }

        if (bgmToPlay != BgmType.None)
        {
            SoundManager.Instance.PlayBgm(bgmToPlay);
        }
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
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

            SoundManager.Instance.PlaySe(SeType.UIClick);

            nextPageIndicator.SetActive(false);
        }

        // 最後のページ表示後、最後のクリックを待ってから終了
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        SoundManager.Instance.PlaySe(SeType.UIClick);

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
        SoundManager.Instance.PlaySe(SeType.UIClick);

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
        // ★★ ランキングの判定を削除し、常にタイトル画面へ遷移するように修正 ★★
        SceneController.Instance.LoadTitleScene();
    }
}