using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField, Header("移動速度")]
    private float _moveSpeed;
    [SerializeField, Header("ジャンプ速度")]
    private float _jumpSpeed;
    [SerializeField, Header("体力")]
    private float _hp;
    [SerializeField, Header("無敵時間")]
    private float _damageTime;
    [SerializeField, Header("点滅時間")]
    private float _flashTime;

    private Vector2 _inputDirection; // 入力された移動方向
    private Rigidbody2D _rigid; // プレイヤーの物理挙動用
    private Animator _anim; // プレイヤーのアニメーション制御用
    private SpriteRenderer _spriteRenderer; // プレイヤーの見た目を変更するためのスプライトレンダラー
    private bool _bJump; // ジャンプ中であるかを判定するフラグ

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _bJump = false; // 初期状態では地面にいるとみなす

    }

    // Update is called once per frame
    void Update()
    {
        _Move(); // 移動処理
        _LookMoveDirec(); // 向きを変更
        Debug.Log(_hp); // 地面との接触判定
        _HitFloor(); // HPをデバッグログに表示（開発用）
    }

    // 左右移動処理
    private void _Move()
    {
        if (_bJump) // 空中では移動制限（ジャンプ中）
        {
            return;
        }
        // 入力方向に応じた速度を設定
        _rigid.linearVelocity = new Vector2(_inputDirection.x * _moveSpeed, _rigid.linearVelocity.y);
        // アニメーション：左右に動いているかを判定して「Walk」アニメーション再生
        _anim.SetBool("Walk", _inputDirection.x != 0.0f);
    }

    // プレイヤーの見た目を進行方向に合わせて反転する処理
    private void _LookMoveDirec()
    {
        if (_inputDirection.x > 0.0f)
        {
            transform.eulerAngles = Vector3.zero; // 右向き
        }
        else if (_inputDirection.x < 0.0f)
        {
            transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f); // 左向き
        }
    }

    // 他のコライダーとの衝突処理（敵やゴール）
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "Floor")
        //{
        //    _bJump = false;
        //    _anim.SetBool("Jump", _bJump);
        //}
        if (collision.gameObject.tag == "Enemy")
        {
            _HitEnemy(collision.gameObject); // 敵との接触処理
        }
        else if (collision.gameObject.tag == "Goal")
        {
            // ゴールに到達した場合：ゲームクリア処理を呼び出す
            MainManager.Instance.ShowGameClearUI();
            enabled = false; // Playerスクリプトを無効化
            GetComponent<PlayerInput>().enabled = false; // 入力も無効化
        }
    }

    // 地面に接しているかを判定する処理
    private void _HitFloor()
    {
        int layerMask = LayerMask.GetMask("Floor"); // Floorレイヤーのみ対象
        // 足元の中心位置を基準に、BoxCastで接地判定
        Vector3 rayPos = transform.position - new Vector3(0.0f, transform.lossyScale.y / 2.0f);
        Vector3 raySize = new Vector3(transform.lossyScale.x - 0.1f, 0.1f);
        RaycastHit2D rayHit = Physics2D.BoxCast(rayPos, raySize, 0.0f, Vector2.zero, 0.0f, layerMask);
        if (rayHit.transform == null)
        {
            _bJump = true; // 接地していない＝空中
            _anim.SetBool("Jump", _bJump); // ジャンプアニメーション再生
            return;
        }
        if (rayHit.transform.tag == "Floor" && _bJump)
        {
            _bJump = false; // 接地中
            _anim.SetBool("Jump", _bJump); // ジャンプ終了アニメーション
        }
    }

    // 敵との接触処理（上から踏んだか、ダメージを受けるか）
    private void _HitEnemy(GameObject enemy)
    {
        float halfScaleY = transform.lossyScale.y / 2.0f;
        float enemyHalfScaleY = enemy.transform.lossyScale.y / 2.0f;
        // プレイヤーが敵より上から接触していれば踏みつけ成功
        if (transform.position.y - (halfScaleY - 0.1f) >= enemy.transform.position.y + (enemyHalfScaleY - 0.1f))
        {
            Destroy(enemy); // 敵を倒す
            _rigid.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse); // 反動ジャンプ
        }
        else
        {
            // 敵の攻撃を受けた場合
            enemy.GetComponent<Enemy>().PlayerDamege(this); // プレイヤーにダメージを与える
            gameObject.layer = LayerMask.NameToLayer("PlayerDamage"); // 一時的に当たり判定を無効化
            StartCoroutine(_Damage()); // 無敵（点滅）処理を開始
        }
    }

    // 無敵状態の点滅処理
    IEnumerator _Damage()
    {
        Color color = _spriteRenderer.color;
        for (int i = 0; i < _damageTime; i++)
        {
            yield return new WaitForSeconds(_flashTime);
            _spriteRenderer.color = new Color(color.r, color.g, color.b, 0.0f); // スプライトを透明に

            yield return new WaitForSeconds(_flashTime);
            _spriteRenderer.color = new Color(color.r, color.g, color.b, 1.0f); // スプライトを表示
        }
        _spriteRenderer.color = color; // 元の色に戻す
        gameObject.layer = LayerMask.NameToLayer("Default"); // 通常のレイヤーに戻す
    }

    // HPが0になったかどうかを確認し、削除処理を実行
    private void _Dead()
    {
        if (_hp <= 0)
        {
            Destroy(gameObject); // プレイヤーを削除（ゲームオーバー扱い）
        }
    }

    // プレイヤーがカメラ外に出た際の処理
    private void OnBecameInvisible()
    {
        Camera camera = Camera.main;
        // カメラ外かどうかを確認し、削除処理を実行
        if (camera.name == "Main Camera" && camera.transform.position.y > transform.position.y)
        {
            Destroy(gameObject); // プレイヤーを削除（ゲームオーバー扱い）
        }
    }

    // 入力：移動
    public void _OnMove(InputAction.CallbackContext context)
    {
        _inputDirection = context.ReadValue<Vector2>(); // 入力された方向を記録
    }

    // 入力：ジャンプ
    public void _OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed || _bJump)
        {
            return; // 入力が成立していない or 空中でのジャンプは禁止
        }

        _rigid.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse); // ジャンプ力を加える
        //_bJump = true;
        //_anim.SetBool("Jump", _bJump);
    }

    // プレイヤーが敵からダメージを受けたときにHPを減らす処理
    public void Damage(int damage)
    {
        _hp = Mathf.Max(_hp - damage, 0); // HPを減少（0以下にならない）
        _Dead(); // HPが0なら死亡処理
    }

    // 現在のHPを外部から取得する
    public int GetHP()
    {
        return (int)_hp;
    }
}
