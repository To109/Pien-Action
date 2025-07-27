using UnityEngine;

using UnityEngine;

public class TimeExtendItem : ItemBase // ItemBaseを継承
{
    [SerializeField]
    private float timeToAdd = 10f; // 延長する時間（秒）

    // ItemBaseのApplyEffectメソッドの具体的な中身を記述
    protected override void ApplyEffect(Player player)
    {
        // ★★ 呼び出し先をMainManagerからGameManagerに変更 ★★
        GameManager.Instance.AddTime(timeToAdd);
    }
}