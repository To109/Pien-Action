using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // �V���O���g������
    public static SceneController Instance { get; private set; }

    void Awake()
    {
        // �V���O���g���̐ݒ�
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

    // --- ���S�Ȑ�p���\�b�h�i�R�[�h����Ăяo���p�j---

    public void LoadTitleScene()
    {
        SoundManager.Instance.PlayBgm(BgmType.Title);
        SceneManager.LoadScene("TitleScene"); // �V�[�����͎��ۂ̃t�@�C�����ɍ��킹��
    }

    public void LoadPlayScene()
    {
        // �Q�[���J�n�O�̃f�[�^�Z�b�g�A�b�v�Ȃǂ�GameManager���s��
        SceneManager.LoadScene("PlayScene");
    }

    public void LoadRankingScene()
    {
        SceneManager.LoadScene("RankingScene");
    }

    // ���̐�p���\�b�h���K�v�ɉ����Ă����ɒǉ�...


    // --- �ėp�I�ȃ��\�b�h�iUI�{�^����OnClick()����Ăяo���p�j---

    /// <summary>
    /// �����Ŏw�肳�ꂽ���O�̃V�[�������[�h���܂��B
    /// </summary>
    /// <param name="sceneName">���[�h�������V�[���̃t�@�C����</param>
    public void ChangeScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("�V�[�������w�肳��Ă��܂���I");
            return;
        }
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// 指定されたエンディング種別でエンディングシーンをロードします
    /// </summary>
    public void LoadEndingScene() // ★★ 引数を削除 ★★
    {
        // GameManagerにデータが保存されているので、ここではシーンをロードするだけ
        SceneManager.LoadScene("EndingScene");
    }
}