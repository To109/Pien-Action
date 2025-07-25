using UnityEngine;

// abstract（抽象）を付けることで、このクラス自体はオブジェクトにアタッチできず、
// 必ず継承して使うクラスであることを示す
public abstract class ItemBase : MonoBehaviour
{
    [Header("効果音")]
    [SerializeField]
    private SeType pickupSound = SeType.ItemGet; // アイテム取得時のSE

    // プレイヤーが触れた時に一度だけ呼ばれる処理
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 触れた相手がプレイヤーでなければ何もしない
        if (!other.CompareTag("Player"))
        {
            return;
        }

        // プレイヤーの参照を取得
        Player player = other.GetComponent<Player>();
        if (player == null) return;

        // --- ここから共通処理 ---

        // 1. アイテム固有の効果を発動する（具体的な内容はサブクラスに任せる）
        ApplyEffect(player);

        // 2. 効果音を再生する
        SoundManager.Instance.PlaySe(pickupSound);

        // 3. 自分自身を非表示にする（二度と使えないようにする）
        gameObject.SetActive(false);
    }

    /// <summary>
    /// アイテム固有の効果を実装するための抽象メソッド。
    /// このクラスを継承したクラスは、必ずこの中身を実装する必要がある。
    /// </summary>
    /// <param name="player">接触したプレイヤー</param>
    protected abstract void ApplyEffect(Player player);
}