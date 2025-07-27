using UnityEngine;
using UnityEngine.UI;

public class GameClearUIManager : MonoBehaviour
{
    [SerializeField]
    private Button nextButton; // NEXTボタンへの参照

    private Player player; // プレイヤーの参照

    void Start()
    {
        // プレイヤーの参照を取得
        player = FindObjectOfType<Player>();
        
        // NEXTボタンがクリックされたらOnNextButtonClickedメソッドを呼ぶ
        nextButton.onClick.AddListener(OnNextButtonClicked);
    }

    /// <summary>
    /// NEXTボタンが押された時の処理
    /// </summary>
    private void OnNextButtonClicked()
    {
        if (player == null) return;

        // 1. プレイヤーの現在のライフ状態を取得
        EndingType endingType = player.GetLifeStateForEnding();

        // ★★ 以下の2行を修正・追加 ★★
        // 2. GameManagerに、次に再生すべきエンディングの種類を記憶させる
        GameManager.Instance.EndingToPlay = endingType;
        
        // 3. SceneControllerにエンディングシーンへの遷移を依頼する
        SceneController.Instance.LoadEndingScene(); // 引数は不要になる
    }
}