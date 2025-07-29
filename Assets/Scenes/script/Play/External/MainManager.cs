using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    // �V���O���g���C���X�^���X�i�ǂ�����ł��A�N�Z�X�\�j
    public static MainManager Instance { get; private set; }

    // �Q�[���I�[�o�[���ɕ\������UI
    [SerializeField, Header("�Q�[���I�[�o�[UI")]
    private GameObject _gameOverUI;
    // �Q�[���N���A���ɕ\������UI
    [SerializeField, Header("�Q�[���N���AUI")]
    private GameObject _gameClearUI;

    private GameObject _player; // �v���C���[�I�u�W�F�N�g�̎Q��
    private bool _bShowUI;  // �Q�[���I�[�o�[�ƃN���A�̔���̎Q��

    // �I�u�W�F�N�g��������1��Ă΂��
    private void Awake()
    {
        // �V���O���g���̃C���X�^���X�����ݒ�Ȃ玩�g��o�^
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // ���łɕʂ̃C���X�^���X�����݂���ꍇ�͎��g��j���i�d���h�~�j
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // �V�[��������Player��T���ĎQ�Ƃ�ێ�
        _player = FindObjectOfType<Player>().gameObject;
        _bShowUI = false; // UI�͂܂��\�����Ă��Ȃ�
        //FindObjectOfType<Fade>().FadeStart(_MainStart); // �t�F�[�h���o������������Q�[���J�n���������s
        //_player.GetComponent<Player>().enabled = false; // �Q�[���J�n�O�̓v���C���[����𖳌���
        // �G�̃X�|�[����������
        //foreach (EnemySpawner enemySpawner in FindObjectsOfType<EnemySpawner>())
        //{
        //    enemySpawner.enabled = false;
        //}
    }

        // �t�F�[�h�I����ɌĂяo�����Q�[���J�n����
        //private void _MainStart()
        //{
        //    // �v���C���[�̕\����L���ɂ��A����\�ɂ���
        //    _player.GetComponent<Renderer>().enabled = true;
        //    // �G�̃X�|�[����L����
        //    foreach (EnemySpawner enemySpawner in FindObjectsOfType<EnemySpawner>())
        //    {
        //        enemySpawner.enabled = true;
        //    }
        //}

    void Update()
    {
        _ShowGameOverUI(); // �v���C���[����������Q�[���I�[�o�[UI�\��
        // _ShowGameClearUI();
    }

    // �v���C���[����������Q�[���I�[�o�[UI��\�����鏈��
    private void _ShowGameOverUI()
    {
        // �v���C���[�����݂��Ă���ꍇ�͉������Ȃ�
        if (_player != null && _player)
        {
            return;
        }

        SoundManager.Instance.PlayBgm(BgmType.GameOver);
        
        // �v���C���[�����݂��Ă��Ȃ��ꍇ�̓Q�[���I�[�o�[UI��\��
        _gameOverUI.SetActive(true);
        _bShowUI = true;
    }

    // �O������Ăяo���ăQ�[���N���AUI��\������֐�
    public void ShowGameClearUI()
    {
        _gameClearUI.SetActive(true);
        _bShowUI = true;
    }

    // �����ꂽ�{�^���̏����擾
    public void OnRestart(InputAction.CallbackContext context)
    {
        // �Q�[���I�[�o�[��N���A�o�Ȃ���Ή������Ȃ�
        if (!_bShowUI || !context.performed)
        {
            return;
        }
        // ���݂̃V�[���ɍēǂݍ��݂�����
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
/// <summary>
    /// UIボタンから呼び出すためのリスタート処理
    /// </summary>
    public void RestartScene()
    {
        // 現在のシーンを再読み込みする
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
} // ← この括弧の前に追記
