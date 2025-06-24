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
    private Animator _anim; // 敵のAnimatorコンポーネント
    private Vector2 _moveDirection;  // 現在の移動方向
    private bool _bFloor; // 床に乗っているかのフラグ

    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>(); // Rigidbody2Dを取得
        _anim = GetComponent<Animator>(); // Animatorを取得
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

    // 移動処理
    private void _Move()
    {
        // 空中にいる場合は移動しない
        if (!_bFloor)
        {
            return;
        }
        // 移動方向に速度を設定（Y方向の速度は維持）
        _rigid.linearVelocity = new Vector2(_moveDirection.x * _moveSpeed, _rigid.linearVelocity.y);
    }

    // 壁に当たったら移動方向を反転する処理
    private void _ChangeMoveDirection()
    {
        // 敵の半分のサイズ（スケール）を取得
        Vector2 halfSize = transform.lossyScale / 2.0f;
        // Floorレイヤーに対してレイを飛ばす
        int layerMask = LayerMask.GetMask("Floor");
        RaycastHit2D ray = Physics2D.Raycast(transform.position, -transform.right, halfSize.x + 0.1f, layerMask);
        
        // レイに何も当たらなかった場合は処理終了
        if (ray.transform == null)
        {
            return;
        }
        // Floorに当たった場合は移動方向を反転
        if (ray.transform.tag == "Floor")
        {
            _moveDirection = -_moveDirection;
        }
    }

    // 向きを移動方向に合わせて反転
    private void _LookMoveDirection()
    {
        if (_moveDirection.x < 0.0f)
        {
            transform.eulerAngles = Vector3.zero; // 左向き
        }
        else if (_moveDirection.x > 0.0f)
        {
            transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f); // 右向き（Y軸回転）
        }
    }

    // 足元に床があるかを判定する処理
    private void _HitFloor()
    {
        int layerMask = LayerMask.GetMask("Floor");
        // レイの開始位置（足元）
        Vector3 rayPos = transform.position - new Vector3(0.0f, transform.lossyScale.y / 2.0f);
        // ボックスのサイズ（横幅は少し小さめ、縦は0.1）
        Vector3 raySize = new Vector3(transform.lossyScale.x - 0.1f, 0.1f);
        // BoxCastで床との接触を判定
        RaycastHit2D rayHit = Physics2D.BoxCast(rayPos, raySize, 0.0f, Vector2.zero, 0.0f, layerMask);

        if (rayHit.transform == null)
        {
            // 床に接していなければ空中状態とし、待機アニメーションを再生
            _bFloor = false;
            _anim.SetBool("Idle", true);
            return;
        }
        else if (rayHit.transform.tag == "Floor" && !_bFloor)
        {
            // 再び床に乗った場合、フラグを更新しアニメーションを戻す
            _bFloor = true;
            _anim.SetBool("Idle", false);
        }
    }

    // プレイヤーにダメージを与える処理
    public void PlayerDamege(Player player)
    {
        player.Damage(_attackPower);　// プレイヤーに攻撃力分のダメージを与える
    }
}
