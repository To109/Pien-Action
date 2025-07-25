using UnityEngine;

public class PatrolEnemy : MonoBehaviour
{
    [Header("移動設定")]
    [SerializeField]
    private float moveSpeed = 2f; // この敵の移動スピード

    [Header("巡回地点")]
    [SerializeField]
    private Transform leftPoint;  // 左の折り返し地点
    [SerializeField]
    private Transform rightPoint; // 右の折り返し地点

    private Transform currentTarget; // 現在の目標地点
    private SpriteRenderer spriteRenderer; // 画像を左右反転させるためのコンポーネント

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        // 最初は右の地点を目指す
        currentTarget = rightPoint;
    }

    void Update()
    {
        // 目標地点が設定されていなければ何もしない
        if (currentTarget == null) return;

        // 目標地点に向かって移動
        transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, moveSpeed * Time.deltaTime);

        // 目標地点に到達したら、目標を切り替える
        if (Vector2.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            if (currentTarget == rightPoint)
            {
                currentTarget = leftPoint;
            }
            else
            {
                currentTarget = rightPoint;
            }
        }

        // 移動方向に応じて画像の向きを変える
        FlipSprite();
    }

    // 画像の向きを制御するメソッド
    private void FlipSprite()
    {
        if (currentTarget == rightPoint)
        {
            // 右に向かう時（デフォルトの左向き画像を左右反転させる）
            spriteRenderer.flipX = true;
        }
        else
        {
            // 左に向かう時（デフォルトの左向き画像）
            spriteRenderer.flipX = false;
        }
    }

    // プレイヤーに接触した時の処理
    void OnCollisionEnter2D(Collision2D collision)
    {
        // 衝突した相手がPlayerかどうかをタグで確認
        if (collision.gameObject.CompareTag("Player"))
        {
            // Playerにダメージを与える（PlayerスクリプトにDamageメソッドがあると想定）
            collision.gameObject.GetComponent<Player>().Damage(1);
        }
    }
}