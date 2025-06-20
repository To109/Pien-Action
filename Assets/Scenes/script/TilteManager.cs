using UnityEngine;
using UnityEngine.InputSystem; // 新しいInput System用
using UnityEngine.SceneManagement; // シーン遷移用

public class TilteManager : MonoBehaviour
{
    private bool _bStart;  // ゲーム開始が可能かどうかのフラグ
    private Fade _fade;    // Fade コンポーネントの参照

    void Start()
    {
        _bStart = false;

        // 同じ GameObject にアタッチされている Fade スクリプトを取得
        _fade = GetComponent<Fade>();

        // フェード演出を開始し、完了後に _TitleStart() を実行する
        _fade.FadeStart(_TitleStart);
    }

    void Update()
    {
        
    }

    // タイトル画面でのフェードインが完了したときに呼び出される
    private void _TitleStart()
    {
        _bStart = true; // スペースキー受付を許可
    }

    // シーン遷移を実行する関数
    private void _ChangeScene()
    {
        // プレイヤー用シーンへ遷移
        SceneManager.LoadScene("PlayerScenes");
    }

    // スペースキーが押されたときに呼び出される（Input Systemのイベント）
    public void OnSpaceClick(InputAction.CallbackContext context)
    {
        // フェード完了済み かつ 入力が完了したとき（押し終わりでなく押された瞬間）
        if (!context.performed && _bStart)
        {
            // フェードを開始し、完了後に _ChangeScene() を実行
            _fade.FadeStart(_ChangeScene);

            _bStart = false; // 二重入力防止
        }
    }
}
