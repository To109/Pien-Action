using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// --- このファイル内で使用するデータ定義 ---

// BGMの種類を定義
public enum BgmType
{
    None,
    Title,
    Play,
    GameClear,
    GameOver
}

// SEの種類を定義
public enum SeType
{
    Jump,
    Attack,
    PlayerDamage,
    EnemyDamage,
    ItemGet,
    LevelUp,
    UIClick
}

// BGMの種類とオーディオクリップを紐付けるためのクラス
[System.Serializable]
public class BgmSoundMapping
{
    public BgmType bgmType;
    public AudioClip audioClip;
}

// SEの種類とオーディオクリップを紐付けるためのクラス
[System.Serializable]
public class SeSoundMapping
{
    public SeType seType;
    public AudioClip audioClip;
}


// --- SoundManager本体 ---

public class SoundManager : MonoBehaviour
{
    // シングルトン実装
    public static SoundManager Instance { get; private set; }

    // === インスペクタから設定する項目 ===
    [Header("BGMのリスト")]
    [SerializeField]
    private List<BgmSoundMapping> bgmClips;

    [Header("SEのリスト")]
    [SerializeField]
    private List<SeSoundMapping> seClips;

    // === コンポーネント参照 ===
    private AudioSource bgmSource;
    private AudioSource seSource;

    // === 内部データ ===
    public int BgmVolumeLevel { get; private set; }
    public int SeVolumeLevel { get; private set; }

    void Awake()
    {
        // シングルトンパターンの実装
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーン遷移しても破棄されないようにする

            // BGM用とSE用のAudioSourceをこのGameObjectに自動で追加する
            bgmSource = gameObject.AddComponent<AudioSource>();
            seSource = gameObject.AddComponent<AudioSource>();

            bgmSource.loop = true; // BGMはループ再生を基本とする

            // 保存されている音量設定を読み込む
            LoadVolumeSettings();
        }
        else
        {
            Destroy(gameObject); // 既にインスタンスがあれば自身を破棄
        }
    }

    /// <summary>
    /// 指定された種類のBGMを再生します。
    /// </summary>
    public void PlayBgm(BgmType bgmType)
    {
        // リストから対応するオーディオクリップを探す
        AudioClip clip = bgmClips.FirstOrDefault(m => m.bgmType == bgmType)?.audioClip;

        if (clip == null)
        {
            Debug.LogWarning("BGMが見つかりません: " + bgmType);
            bgmSource.Stop();
            return;
        }

        // もし違う曲が再生中なら、新しい曲に差し替えて再生
        if (bgmSource.clip != clip || !bgmSource.isPlaying)
        {
            bgmSource.clip = clip;
            bgmSource.Play();
        }
    }

    /// <summary>
    /// 指定された種類のSEを再生します。
    /// </summary>
    public void PlaySe(SeType seType)
    {
        // リストから対応するオーディオクリップを探す
        AudioClip clip = seClips.FirstOrDefault(m => m.seType == seType)?.audioClip;

        if (clip != null)
        {
            // PlayOneShotを使うことで、他のSEやBGMを止めずに再生できる
            seSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("SEが見つかりません: " + seType);
        }
    }

    /// <summary>
    /// BGMの音量レベル（0-5段階）を設定します。
    /// </summary>
    public void SetBgmVolume(int level)
    {
        BgmVolumeLevel = Mathf.Clamp(level, 0, 5); // 0-5の範囲に補正
        bgmSource.volume = BgmVolumeLevel / 5.0f; // 0.0-1.0のfloat値に変換
        SaveVolumeSettings();
    }

    /// <summary>
    /// SEの音量レベル（0-5段階）を設定します。
    /// </summary>
    public void SetSeVolume(int level)
    {
        SeVolumeLevel = Mathf.Clamp(level, 0, 5); // 0-5の範囲に補正
        seSource.volume = SeVolumeLevel / 5.0f;
        SaveVolumeSettings();
    }

    private void SaveVolumeSettings()
    {
        // GameSettingsクラスに保存処理を依頼する
        // GameSettings.SaveVolume(BgmVolumeLevel, SeVolumeLevel);
    }

    private void LoadVolumeSettings()
    {
        // GameSettingsクラスから読み込み処理を依頼する
        // var (bgmLevel, seLevel) = GameSettings.LoadVolume();
        // SetBgmVolume(bgmLevel);
        // SetSeVolume(seLevel);

        // GameSettingsが未実装の場合は、デフォルト値を設定
        SetBgmVolume(5);
        SetSeVolume(5);
    }
}