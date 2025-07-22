using UnityEngine;

public class LifeRecoveryItem : ItemBase // ItemBaseを継承
{
    [SerializeField]
    private int healAmount = 1; // 回復量

    // ItemBaseのApplyEffectメソッドの具体的な中身を記述
    protected override void ApplyEffect(Player player)
    {
        // プレイヤーのHealメソッドを呼び出す
        //player.Heal(healAmount);
    }
}