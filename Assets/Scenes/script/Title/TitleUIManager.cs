using UnityEngine;
using UnityEngine.UI; // Buttonを使うために必要

public class TitleUIManager : MonoBehaviour
{
    [Header("難易度選択ボタン")]
    [SerializeField] private Button easyButton;
    [SerializeField] private Button normalButton;
    [SerializeField] private Button hardButton;
    
    // 他のボタンも必要ならここに追加
    // [SerializeField] private Button howToPlayButton;
    // [SerializeField] private Button rankingButton;
    // [SerializeField] private Button optionsButton;


    void Start()
    {
        // 各ボタンに、クリックされた時の処理を登録します
        // () => ... の部分は、クリック時にどのメソッドをどの引数で呼ぶかを指定しています
        easyButton.onClick.AddListener(() => OnDifficultySelected(Difficulty.Easy));
        normalButton.onClick.AddListener(() => OnDifficultySelected(Difficulty.Normal));
        hardButton.onClick.AddListener(() => OnDifficultySelected(Difficulty.Hard));
        
        // 他ボタンの処理も同様に登録
        // howToPlayButton.onClick.AddListener(OnHowToPlayClicked);
    }

    /// <summary>
    /// 難易度選択ボタンが押された時に呼ばれる共通のメソッド
    /// </summary>
    private void OnDifficultySelected(Difficulty selectedDifficulty)
    {
        // 1. クリック音を鳴らす（SoundManagerが存在すれば）
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySe(SeType.UIClick);
        }

        // 2. 選択された難易度をDifficultyManagerに記憶させる
        if (DifficultyManager.Instance != null)
        {
            DifficultyManager.Instance.SelectDifficulty(selectedDifficulty);
        }

        // 3. SceneControllerにPlaySceneへの遷移を依頼する
        if (SceneController.Instance != null)
        {
            SceneController.Instance.LoadPlayScene();
        }
    }

    // private void OnHowToPlayClicked()
    // {
    //     // 遊び方画面の処理
    // }
}