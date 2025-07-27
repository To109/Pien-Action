// --- 1. usingディレクティブ (必ず一番上にまとめて書きます) ---
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// --- 2. データ定義 (クラス定義の前に書くと見やすいです) ---

// このファイル、または別のファイルで一度だけ定義されていればOK
public enum Difficulty
{
    Easy,
    Normal,
    Hard
}

// 難易度ごとのパラメータを保持するクラス
[System.Serializable]
public class DifficultyParameter
{
    public Difficulty difficulty;
    public BgmType stageBgm;
    public int initialPlayerHealth;
    public float initialTimeLimit;
}


// --- 3. メインのクラス定義 ---
public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }

    [SerializeField]
    private List<DifficultyParameter> difficultySettings;
    
    public Difficulty CurrentDifficulty { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SelectDifficulty(Difficulty difficulty)
    {
        CurrentDifficulty = difficulty;
    }

    public DifficultyParameter GetCurrentDifficultyParameters()
    {
        // 現在選択されている難易度に一致するパラメータ設定をリストから探して返す
        return difficultySettings.FirstOrDefault(p => p.difficulty == CurrentDifficulty);
    }
}