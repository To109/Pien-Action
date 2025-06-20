using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Fade : MonoBehaviour
{
    // フェードのモード（フェードイン or フェードアウト）
    enum Mode
    {
        FadeIn,  // 画面が明るくなる
        FadeOut, // 画面が暗くなる
    }

    [SerializeField, Header("フェードの時間")]
    private float _fadeTime; // フェードの所要時間（秒）
    [SerializeField, Header("フェードの種類")]
    private Mode _mode; // 初期のフェードモード

    private bool _bFade; // フェード中かどうかのフラグ
    private float _fadeCount; // 現在のフェード時間カウント
    private Image _image; // フェード対象の UI Image コンポーネント
    private UnityEvent _onFadeComplete = new UnityEvent(); // フェード完了時に呼び出されるイベント

    void Start()
    {
        // アタッチされている Image コンポーネントを取得
        _image = GetComponent<Image>();

        // 初期モードに応じてカウント初期化
        switch (_mode)
        {
            case Mode.FadeIn:
                _fadeCount = _fadeTime; // フェードインは最大からスタート
                break;
            case Mode.FadeOut:
                _fadeCount = 0; // フェードアウトは0からスタート
                break;
        }
    }

    void Update()
    {
        _Fade(); // フェード処理を毎フレーム実行
    }

    // モードに応じてフェード処理を実行
    private void _Fade()
    {
        if (!_bFade)
        {
            return; // フェード中でなければ何もしない
        }

        switch (_mode)
        {
            case Mode.FadeIn:
                _FadeIn(); // フェードイン処理
                break;
            case Mode.FadeOut:
                _FadeOut(); // フェードアウト処理
                break;
        }

        // アルファ値を現在のカウントに応じて更新
        float alpha = _fadeCount / _fadeTime;
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, alpha);
    }

    // フェードイン処理（徐々に透明に）
    private void _FadeIn()
    {
        _fadeCount -= Time.deltaTime; // カウントを減らす

        if (_fadeCount <= 0)
        {
            _mode = Mode.FadeOut; // 次のモードに切り替え
            _bFade = false;       // フェード終了
            _onFadeComplete.Invoke(); // フェード完了イベントを呼び出す
        }
    }

    // フェードアウト処理（徐々に不透明に）
    private void _FadeOut()
    {
        _fadeCount += Time.deltaTime; // カウントを増やす

        if (_fadeCount >= _fadeTime)
        {
            _mode = Mode.FadeIn;  // 次のモードに切り替え
            _bFade = false;       // フェード終了
            _onFadeComplete.Invoke(); // フェード完了イベントを呼び出す
        }
    }

    // 外部からフェード処理を開始し、完了時に処理を登録する
    public void FadeStart(UnityAction listener)
    {
        if (_bFade)
        {
            return; // フェード中なら無視
        }

        _bFade = true; // フェード開始
        _onFadeComplete.AddListener(listener); // フェード完了時に呼び出すリスナーを登録
    }
}
