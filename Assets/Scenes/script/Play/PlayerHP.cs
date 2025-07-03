using UnityEngine;
using UnityEngine.UI;

// PlayerHPという名前のクラス（コンポーネント）を定義
// このスクリプトは、プレイヤーのHPをUIで表示する役割を持ちます
public class PlayerHP : MonoBehaviour
{
    // [SerializeField]を付けると、privateな変数でもUnityエディタのインスペクターに表示され、
    // そこから値を設定（この場合はHPアイコンのプレハブをドラッグ＆ドロップ）できるようになる
    // [Header("HPアイコン")]は、インスペクターに見出しを表示して分かりやすくするためのもの
    [SerializeField, Header("HPアイコン")]
    private GameObject _playerIcon; // HPアイコンの元となるゲームオブジェクト（プレハブ）を格納する変数

    private Player _player;     
    private int _beforeHP;      // 前のフレームのHPを保存しておくための変数。HPに変化があったかを確認するために使う

    // ゲームが開始した時に一度だけ呼ばれるメソッド
    void Start()
    {
        // シーン内からPlayerコンポーネントを持つオブジェクトを探し出して、_player変数にその情報を入れる
        _player = FindAnyObjectByType<Player>();

        // _playerの現在のHPを取得して、_beforeHPに最初の値を設定
        _beforeHP = _player.GetHP();

        // 最初にHPアイコンを生成するメソッドを呼び出す
        _CreateHPIcon();
    }

    // HPアイコンを生成するためのメソッド
    private void _CreateHPIcon()
    {
        // プレイヤーの現在のHPの数だけ、forループ（繰り返し処理）を行う
        for (int i = 0; i < _player.GetHP(); i++)
        {
            // インスペクターで設定した_playerIconのプレハブを元に、新しいゲームオブジェクトをシーン上に生成
            GameObject _playerHPObj = Instantiate(_playerIcon);

            // 生成したHPアイコンの親要素を、このスクリプトがアタッチされているオブジェクトに設定
            // これにより、UIの階層が整理され、アイコンがCanvas内の適切な場所に配置される
            _playerHPObj.transform.SetParent(transform, false);
        }
    }

    // 毎フレーム（1秒間に何十回も）呼ばれ続けるメソッド
    void Update()
    {
        // HPアイコンの表示を更新するメソッドを呼び出す
        _ShowHPIcon();
    }

    // HPアイコンの表示を更新するためのメソッド
    private void _ShowHPIcon()
    {
        // もし、現在のHPと前のフレームのHPが同じだったら、 HPに変化がないということなのでこの後の処理は行わずに終了
        if (_beforeHP == _player.GetHP())
        {
            return;
        }

        // このスクリプトがアタッチされているオブジェクトの子要素の中から、すべてのImageコンポーネントを探し出して配列に入れる
        Image[] icons = transform.GetComponentsInChildren<Image>();

        // 取得したアイコンの数だけ、forループ（繰り返し処理）を行う
        for (int i = 0; i < icons.Length; i++)
        {
            // アイコンを表示するかどうかを決める
            // i（アイコンの番号）がプレイヤーの現在のHPより小さい場合だけ、アイコンをアクティブ（表示状態）にする
            // 例：HPが3の場合、iが0, 1, 2のアイコンは表示され、iが3以上のアイコンは非表示になる
            icons[i].gameObject.SetActive(i < _player.GetHP());
        }

        // 処理が終わったら、現在のHPを_beforeHPに保存し、次のフレームでの比較に備える
        _beforeHP = _player.GetHP();
    }
}