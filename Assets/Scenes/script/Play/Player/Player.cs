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
    
    // ★★ _hpのSerializeFieldを削除し、最大値と現在値を分ける ★★
    private float _maxHp; // 最大ライフ
    private float _hp;    // 現在のライフ

    [SerializeField, Header("無敵時間")]
    private float _damageTime;
    [SerializeField, Header("点滅時間")]
    private float _flashTime;

    private Vector2 _inputDirection; // 入力された移動方向
    private Rigidbody2D _rigid; // プレイヤーの物理挙動用
    private Animator _anim; // プレイヤーのアニメーション制御用
    private SpriteRenderer _spriteRenderer; // プレイヤーの見た目を変更するためのスプライトレンダラー
    public bool _bJump; // ジャンプ中であるかを判定するフラグ
    private Camera _cachedMainCamera; // カメラの参照をキャッシュするフィールド


    void Awake()
    {
        // 各コンポーネントへの参照を取得
        _cachedMainCamera = Camera.main;
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        // 初期状態では地面にいるとみなす
        _bJump = false;
    }

    void Start()
    {

    }

    void Update()
    {
        _HitFloor(); // 地面との接触判定
        _Move(); // 移動処理
        _LookMoveDirec(); // 向きを変更
        Debug.Log(_hp); // HPをデバッグログに表示（開発用）
    }

    // ★★ ここから下のHealth関連メソッドを修正・追加 ★★

    /// <summary>
    /// GameManagerから初期ライフを設定するためのメソッド
    /// </summary>
    /// <param name="health">初期ライフの値</param>
    public void SetInitialHealth(int health)
    {
        _maxHp = health;
        _hp = _maxHp;
    }

    /// <summary>
    /// プレイヤーが敵からダメージを受けたときにHPを減らす処理
    /// </summary>
    /// <param name="damage">受けるダメージ量</param>
    public void Damage(int damage)
    {
        _hp = Mathf.Max(_hp - damage, 0); // HPを減少（0以下にならない）
        _Dead(); // HPが0なら死亡処理
    }
    
    /// <summary>
    /// アイテムなどでライフを回復するためのメソッド
    /// </summary>
    /// <param name="amount">回復量</param>
    public void Heal(int amount)
    {
        // 回復しても最大ライフは超えないようにする
        _hp = Mathf.Min(_hp + amount, _maxHp);
    }

    /// <summary>
    /// 現在のHPを外部から取得する
    /// </summary>
    /// <returns>現在のHP（整数）</returns>
    public int GetHP()
    {
        return (int)_hp;
    }
    
    #region // --- 既存のメソッド（変更なし） ---

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
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        Bounds bounds = collider.bounds;
        int layerMask = LayerMask.GetMask("Floor");
        float upwardShift = 0.1f;
        Vector2 raySize = new Vector2(bounds.size.x * 0.0f, 0.2f);
        Vector2 rayPos = new Vector2(bounds.center.x, bounds.center.y - bounds.extents.y - raySize.y / 2f + upwardShift);
        Collider2D rayHit = Physics2D.OverlapBox(rayPos, raySize, 0.0f, layerMask);

        if (rayHit == null)
        {
            _bJump = true; // 接地していない＝空中
            _anim.SetBool("Jump", _bJump);
        }
        else
        {
            if (_bJump) // ジャンプ中から接地状態に変わった場合
            {
                _bJump = false; // 接地中
                _anim.SetBool("Jump", _bJump);
            }
        }
    }

    // 敵との接触処理（上から踏んだか、ダメージを受けるか）
    private void _HitEnemy(GameObject enemy)
    {
        float halfScaleY = transform.lossyScale.y / 2.0f;
        float enemyHalfScaleY = enemy.transform.lossyScale.y / 2.0f;
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
        _bJump = true;
        _anim.SetBool("Jump", _bJump);
    }

    private void OnDrawGizmos()
    {
        if (GetComponent<BoxCollider2D>() == null) return;
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        Bounds bounds = collider.bounds;
        float upwardShift = 0.1f;
        Gizmos.color = Color.green;
        Vector2 raySize = new Vector2(bounds.size.x * 0.0f, 0.2f);
        Vector2 rayPos = new Vector2(bounds.center.x, bounds.center.y - bounds.extents.y - raySize.y / 2f + upwardShift);
        Gizmos.DrawWireCube(rayPos, raySize);
    }
    
    
 /// <summary>
    /// ゲームクリア時のエンディング分岐のために、現在のライフ状態を返す
    /// </summary>
    public EndingType GetLifeStateForEnding()
    {
        // 仕様に基づき、ライフの割合を計算して対応するEndingTypeを返す
        if (_hp >= _maxHp) // ライフが満タンの場合
        {
            return EndingType.A;
        }
        else if (_hp >= _maxHp * 0.5f) // ライフが50%以上の場合
        {
            return EndingType.B;
        }
        else // ライフが50%未満の場合
        {
            return EndingType.C;
        }
    }

    #endregion
} // ← この括弧の前に追記