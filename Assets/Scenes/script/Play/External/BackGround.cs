// using UnityEditor.Rendering;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    // 視差効果の強さ（0〜1）
    [SerializeField, Header("視差効果"), Range(0, 1)]
    private float _parallaxEffect;

    private GameObject _camera; // メインカメラの参照
    private float _length;  // 背景画像の幅
    private float _startPosX; // 背景の初期X位置

    void Start()
    {
        _startPosX = transform.position.x; // 初期位置を記録
        _length = GetComponent<SpriteRenderer>().bounds.size.x; // 背景スプライトの幅を取得
        _camera = Camera.main.gameObject;
    }

    // 毎フレームごとに呼び出すのはよくない
    void Update()
    {

    }

    // 一定時間ごとに呼び出す
    private void FixedUpdate()
    {
        _Parallax(); // 視差効果の更新
    }

    // 視差スクロールの処理
    private void _Parallax()
    {
        // temp: 背景のループ処理用
        float temp = _camera.transform.position.x * (1 - _parallaxEffect);
        // dist: 実際に背景を動かす距離（視差分）
        float dist = _camera.transform.position.x * _parallaxEffect;

        // 背景のX位置を更新（視差効果を反映）
        transform.position = new Vector3(_startPosX + dist, transform.position.y, transform.position.z);

        // カメラが背景の端を超えたら背景をループさせる
        if (temp > _startPosX + _length)
        {
            _startPosX += _length; // 右側
        }
        else if (temp < _startPosX - _length)
        {
            _startPosX -= _length; // 左側
        }
    } 
}
