using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        // このオブジェクトがInitializerSceneにある場合のみ、TitleSceneをロードする
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "InitializerScene")
        {
            // SoundManagerのBGM再生などもここで行うと確実
            SoundManager.Instance.PlayBgm(BgmType.Title);

            // TitleSceneへ遷移
            SceneController.Instance.LoadTitleScene();
        }
    }
}
