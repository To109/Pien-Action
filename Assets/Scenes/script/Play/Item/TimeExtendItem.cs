using UnityEngine;

public class TimeExtendItem : ItemBase // ItemBaseを継承
{
    [SerializeField]
    private float timeToAdd = 10f; // 延長する時間（秒）

    // ItemBaseのApplyEffectメソッドの具体的な中身を記述
    protected override void ApplyEffect(Player player)
    {
        // MainManagerのExtendTimeメソッドを呼び出す
        // MainManager.Instance.ExtendTime(timeToAdd);
    }
}