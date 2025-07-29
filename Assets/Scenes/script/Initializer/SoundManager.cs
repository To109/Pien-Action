using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// --- ���̃t�@�C�����Ŏg�p����f�[�^��` ---

// BGM�̎�ނ��`
public enum BgmType
{
    None,
    Title,
    END_A,
    END_B,
    END_C,
    Easy_play,
    Normal_play,
    Hard_play,
    GameOver
}

// SE�̎�ނ��`
public enum SeType
{
    Jump,
    Attack,
    PlayerDamage,
    ItemGet,
    LevelUp,
    UIClick
}

// BGM�̎�ނƃI�[�f�B�I�N���b�v��R�t���邽�߂̃N���X
[System.Serializable]
public class BgmSoundMapping
{
    public BgmType bgmType;
    public AudioClip audioClip;
}

// SE�̎�ނƃI�[�f�B�I�N���b�v��R�t���邽�߂̃N���X
[System.Serializable]
public class SeSoundMapping
{
    public SeType seType;
    public AudioClip audioClip;
}


// --- SoundManager�{�� ---

public class SoundManager : MonoBehaviour
{
    // �V���O���g������
    public static SoundManager Instance { get; private set; }

    // === �C���X�y�N�^����ݒ肷�鍀�� ===
    [Header("BGM�̃��X�g")]
    [SerializeField]
    private List<BgmSoundMapping> bgmClips;

    [Header("SE�̃��X�g")]
    [SerializeField]
    private List<SeSoundMapping> seClips;

    // === �R���|�[�l���g�Q�� ===
    private AudioSource bgmSource;
    private AudioSource seSource;

    // === �����f�[�^ ===
    public int BgmVolumeLevel { get; private set; }
    public int SeVolumeLevel { get; private set; }

    void Awake()
    {
        // �V���O���g���p�^�[���̎���
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �V�[���J�ڂ��Ă��j������Ȃ��悤�ɂ���

            // BGM�p��SE�p��AudioSource������GameObject�Ɏ����Œǉ�����
            bgmSource = gameObject.AddComponent<AudioSource>();
            seSource = gameObject.AddComponent<AudioSource>();

            bgmSource.loop = true; // BGM�̓��[�v�Đ�����{�Ƃ���

            // �ۑ�����Ă��鉹�ʐݒ��ǂݍ���
            LoadVolumeSettings();
        }
        else
        {
            Destroy(gameObject); // ���ɃC���X�^���X������Ύ��g��j��
        }
    }

    /// <summary>
    /// �w�肳�ꂽ��ނ�BGM���Đ����܂��B
    /// </summary>
    public void PlayBgm(BgmType bgmType)
    {
        // ���X�g����Ή�����I�[�f�B�I�N���b�v��T��
        AudioClip clip = bgmClips.FirstOrDefault(m => m.bgmType == bgmType)?.audioClip;

        if (clip == null)
        {
            Debug.LogWarning("BGM��������܂���: " + bgmType);
            bgmSource.Stop();
            return;
        }

        // �����Ⴄ�Ȃ��Đ����Ȃ�A�V�����Ȃɍ����ւ��čĐ�
        if (bgmSource.clip != clip || !bgmSource.isPlaying)
        {
            bgmSource.clip = clip;
            bgmSource.Play();
        }
    }

    /// <summary>
    /// �w�肳�ꂽ��ނ�SE���Đ����܂��B
    /// </summary>
    public void PlaySe(SeType seType)
    {
        // ���X�g����Ή�����I�[�f�B�I�N���b�v��T��
        AudioClip clip = seClips.FirstOrDefault(m => m.seType == seType)?.audioClip;

        if (clip != null)
        {
            // PlayOneShot���g�����ƂŁA����SE��BGM���~�߂��ɍĐ��ł���
            seSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("SE��������܂���: " + seType);
        }
    }

    /// <summary>
    /// BGM�̉��ʃ��x���i0-4�i�K�j��ݒ肵�܂��B
    /// </summary>
    public void SetBgmVolume(int level)
    {
        BgmVolumeLevel = Mathf.Clamp(level, 0, 4); // 0-4�͈̔͂ɕ␳
        bgmSource.volume = BgmVolumeLevel / 4.0f; // 0.0-1.0��float�l�ɕϊ�
        SaveVolumeSettings();
    }

    /// <summary>
    /// SE�̉��ʃ��x���i0-4�i�K�j��ݒ肵�܂��B
    /// </summary>
    public void SetSeVolume(int level)
    {
        SeVolumeLevel = Mathf.Clamp(level, 0, 4); // 0-4�͈̔͂ɕ␳
        seSource.volume = SeVolumeLevel / 4.0f;
        SaveVolumeSettings();
    }

    private void SaveVolumeSettings()
    {
        // GameSettings�N���X�ɕۑ��������˗�����
        // GameSettings.SaveVolume(BgmVolumeLevel, SeVolumeLevel);
    }

    private void LoadVolumeSettings()
    {
        // GameSettings�N���X����ǂݍ��ݏ������˗�����
        // var (bgmLevel, seLevel) = GameSettings.LoadVolume();
        // SetBgmVolume(bgmLevel);
        // SetSeVolume(seLevel);

        // GameSettings���������̏ꍇ�́A�f�t�H���g�l��ݒ�
        SetBgmVolume(1);
        SetSeVolume(4);
    }
}