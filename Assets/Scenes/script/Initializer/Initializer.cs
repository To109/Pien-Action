using UnityEngine;

public class Initializer : MonoBehaviour
{
    void Start()
    {
        // --- ここでゲーム開始時に一度だけ行いたい処理を実行する ---

        // 1. SoundManagerにタイトルBGMの再生を指示する
        //    (BgmType.Titleはご自身のenum定義に合わせてください)
        SoundManager.Instance.PlayBgm(BgmType.Title);

        // 2. SceneControllerにTitleSceneへの遷移を指示する
        SceneController.Instance.ChangeScene("TitleScene");
    }
}