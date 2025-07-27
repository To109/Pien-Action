using UnityEngine;
using UnityEngine.UI;

public class GameOverUIManager : MonoBehaviour
{
    [SerializeField]
    private Button yesButton; // YESボタンへの参照

    [SerializeField]
    private Button noButton;  // NOボタンへの参照

    void Start()
    {
        // YESボタンがクリックされたらOnYesClickedメソッドを呼ぶ
        yesButton.onClick.AddListener(OnYesClicked);

        // NOボタンがクリックされたらOnNoClickedメソッドを呼ぶ
        noButton.onClick.AddListener(OnNoClicked);
    }

    /// <summary>
    /// YESボタン（コンティニュー）が押された時の処理
    /// </summary>
    private void OnYesClicked()
    {
        // SoundManagerでクリック音を鳴らす（推奨）
        SoundManager.Instance.PlaySe(SeType.UIClick);

        // MainManagerにリスタートを依頼する
        MainManager.Instance.RestartScene();
    }

    /// <summary>
    /// NOボタン（タイトルへ）が押された時の処理
    /// </summary>
    private void OnNoClicked()
    {
        // SoundManagerでクリック音を鳴らす（推奨）
        SoundManager.Instance.PlaySe(SeType.UIClick);

        // SceneControllerにタイトル画面への遷移を依頼する
        SceneController.Instance.LoadTitleScene();
    }
}