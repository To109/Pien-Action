using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EndingManager : MonoBehaviour
{
    // シングルトン実装
    public static EndingManager Instance { get; private set; }

    // インスペクタから設定するエンディングのデータリスト
    [SerializeField]
    private List<EndingContent> endingContents;

    void Awake()
    {
        // シングルトンの設定
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンをまたいで存在させる
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 指定された種類のエンディング内容を取得するメソッド
    public EndingContent GetEndingContent(EndingType type)
    {
        // リストの中から、引数で指定されたendingTypeを持つものを探して返す
        return endingContents.FirstOrDefault(content => content.endingType == type);
    }
}