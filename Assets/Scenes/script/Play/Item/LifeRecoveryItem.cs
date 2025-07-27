using UnityEngine;

public class LifeRecoveryItem : ItemBase // ItemBase���p��
{
    [SerializeField]
    private int healAmount = 1; // �񕜗�

    // ItemBase��ApplyEffect���\�b�h�̋�̓I�Ȓ��g���L�q
    protected override void ApplyEffect(Player player)
    {
        // �v���C���[��Heal���\�b�h���Ăяo��
        player.Heal(healAmount);
    }
}