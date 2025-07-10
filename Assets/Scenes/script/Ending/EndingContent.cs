using UnityEngine;
using UnityEngine.UI; // Spriteを使うために必要

// エンディングの種類を定義するenum（列挙型）
public enum EndingType
{
    A,
    B,
    C
}

// エンディングの内容を保持するクラス
// [System.Serializable]を付けると、インスペクタ上に表示・編集できるようになる
[System.Serializable]
public class EndingContent
{
    public EndingType endingType; // このデータがどのエンディングに対応するか
    public Sprite endingCg;       // 表示するCG画像
    [TextArea(5, 10)]           // インスペクタで編集しやすくするための属性
    public string storyText;      // 表示するストーリーテキスト
}