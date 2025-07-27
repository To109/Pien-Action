using UnityEngine;
using System.Linq; // LINQを使うために必要

public class ActivatorByDifficulty : MonoBehaviour
{
    [Tooltip("このオブジェクトが有効になる（出現する）難易度を複数選択できます")]
    [SerializeField]
    private Difficulty[] activeOnDifficulties;

    void Awake()
    {
        // DifficultyManagerが存在しない場合は何もしない
        if (DifficultyManager.Instance == null) return;

        // 現在のゲームの難易度を取得
        Difficulty currentDifficulty = DifficultyManager.Instance.CurrentDifficulty;

        // activeOnDifficultiesリストに現在の難易度が含まれているかチェック
        bool shouldBeActive = activeOnDifficulties.Contains(currentDifficulty);

        // もし含まれていなければ、このGameObjectを非アクティブにする
        if (!shouldBeActive)
        {
            gameObject.SetActive(false);
        }
    }
}