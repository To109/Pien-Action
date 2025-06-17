using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // カメラが振動する合計時間
    [SerializeField, Header("振動する時間")]
    private float _shakeTime;
    // 振動の強さ（幅）
    [SerializeField, Header("振動の大きさ")]
    private float _shakeMagnitude;

    private Player _player; // プレイヤーの参照
    private Vector3 _initPos; // カメラの初期位置
    private float _shakeCount; // 振動時間のカウント
    private int _currentPlayerHP; // 現在のプレイヤーHPを保持

    void Start()
    {
        _player = FindAnyObjectByType<Player>(); // シーン内からプレイヤーを取得
        _currentPlayerHP = _player.GetHP(); // 初期HPを記録
        _initPos = transform.position; // カメラの初期位置を記録
    }

    void Update()
    {
        _ShskeCheck(); // プレイヤーHP変化をチェックし、カメラを揺らすか判定
        _FollowPlayer(); // プレイヤーのX座標に追従
    }

    // プレイヤーのHPが変化したかどうかをチェックして振動開始
    private void _ShskeCheck()
    {
        if (_currentPlayerHP != _player.GetHP())
        {
            _currentPlayerHP = _player.GetHP(); // 最新のHPを更新
            _shakeCount = 0.0f; // 振動タイマーをリセット
            StartCoroutine(_Shake()); // 振動コルーチン開始
        }
    }

    // カメラを一定時間ランダムに揺らすコルーチン
    IEnumerator _Shake()
    {
        Vector3 initPos = transform.position; // 現在のカメラ位置を記録

        while (_shakeCount < _shakeTime)
        {
            // ランダムなX, Y座標にずらして振動を演出
            float x = initPos.x + Random.Range(-_shakeMagnitude, _shakeMagnitude);
            float y = initPos.y + Random.Range(-_shakeMagnitude, _shakeMagnitude);
            transform.position = new Vector3(x, y, initPos.z);

            _shakeCount += Time.deltaTime; // 振動時間を加算

            yield return null; // 次のフレームまで待機
        }

        // 振動が終わったら元の位置に戻す
        transform.position = initPos;
    }

    // プレイヤーのX座標にカメラを追従させる処理（X方向のみ）
    private void _FollowPlayer()
    {
        float x = _player.transform.position.x;
        // カメラのX位置が初期位置より左に行かないよう制限
        x = Mathf.Clamp(x, _initPos.x, Mathf.Infinity);
        // カメラ位置を更新（Y, Zはそのまま）
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
}
