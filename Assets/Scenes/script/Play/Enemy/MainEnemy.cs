using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 敵の移動速度
    [SerializeField, Header("移動速度")]
    private float _moveSpeed;
    // プレイヤーに与えるダメージ量
    [SerializeField, Header("攻撃力")]
    private int _attackPower;

    private Rigidbody2D _rigid; // 敵のRigidbody2Dコンポーネント
    
    // ★★ Animatorの代わりにSpriteRendererを使用 ★★
    private SpriteRenderer _spriteRenderer; 

    private Vector2 _moveDirection;  // 現在の移動方向
    private bool _bFloor; // 床に乗っているかのフラグ

    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>(); // Rigidbody2Dを取得
        
        // ★★ Animatorの取得を削除し、SpriteRendererを取得 ★★
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _moveDirection = Vector2.left; // 初期移動方向は左
        _bFloor = true; // 初期状態では床に乗っているとする
    }

    void Update()
    {
        _Move(); // 移動処理
        _ChangeMoveDirection(); // 壁などに当たったら移動方向を変更
        _LookMoveDirection(); // 移動方向に応じて向きを変える
        _HitFloor(); // 床に接しているかどうかをチェック
    }

    // 移動処理（変更なし）
    private void _Move()
    {
        if (!_bFloor)
        {
            return;
        }
        _rigid.linearVelocity = new Vector2(_moveDirection.x * _moveSpeed, _rigid.linearVelocity.y);
    }

    // 壁に当たったら移動方向を反転する処理（変更なし）
    private void _ChangeMoveDirection()
    {
        Vector2 halfSize = transform.lossyScale / 2.0f;
        int layerMask = LayerMask.GetMask("Floor");
        RaycastHit2D ray = Physics2D.Raycast(transform.position, -transform.right, halfSize.x + 0.1f, layerMask);
        
        if (ray.transform == null)
        {
            return;
        }
        if (ray.transform.tag == "Floor")
        {
            _moveDirection = -_moveDirection;
        }
    }

    // ★★ 向きをSpriteRendererの反転で制御するように修正 ★★
    private void _LookMoveDirection()
    {
        if (_moveDirection.x < 0.0f)
        {
            // 左向きの時、画像の反転を解除 (デフォルト)
            _spriteRenderer.flipX = false;
        }
        else if (_moveDirection.x > 0.0f)
        {
            // 右向きの時、画像をX軸で反転
            _spriteRenderer.flipX = true;
        }
    }

    // ★★ 床判定処理からAnimator関連の記述を削除 ★★
    private void _HitFloor()
    {
        int layerMask = LayerMask.GetMask("Floor");
        Vector3 rayPos = transform.position - new Vector3(0.0f, transform.lossyScale.y / 2.0f);
        Vector3 raySize = new Vector3(transform.lossyScale.x - 0.1f, 0.1f);
        RaycastHit2D rayHit = Physics2D.BoxCast(rayPos, raySize, 0.0f, Vector2.zero, 0.0f, layerMask);

        if (rayHit.transform == null)
        {
            // 床に接していなければ空中状態
            _bFloor = false;
            // _anim.SetBool("Idle", true); // ← アニメーション処理を削除
            return;
        }
        else if (rayHit.transform.tag == "Floor" && !_bFloor)
        {
            // 再び床に乗った場合
            _bFloor = true;
            // _anim.SetBool("Idle", false); // ← アニメーション処理を削除
        }
    }

    // プレイヤーにダメージを与える処理（変更なし）
    public void PlayerDamege(Player player)
    {
        player.Damage(_attackPower);
    }
}