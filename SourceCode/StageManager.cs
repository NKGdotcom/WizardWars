using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static SoundManager;
[System.Serializable]
public class SpawnFourPoint
{
    [Header("�X�|�[��������͈͂�4�p�`�Ƃ��Ĉ͂�")]
    public Transform firstPoint;
    public Transform secondPoint;
    public Transform thirdPoint;
    public Transform fourthPoint;
}
public class StageManager : MonoBehaviour
{
    public static StageManager Instance {  get; private set; }
    public GameObject Player { get; private set; }
    public List<GameObject> NowEnemyList { get => _nowEnemyList; set => _nowEnemyList = value; }
    public bool DuringPlay { get => _duringPlay; set => _duringPlay = value; }
    public int NowEnemyDefeatNum { get => _nowEnemyDefeatNum;set => _nowEnemyDefeatNum = value; }
    public int EnemyNextPhaseNum { get => _enemyNextPhaseNum; set => _enemyNextPhaseNum = value; }
    public int Phase { get => _phase; set => _phase = value; }
    public bool BossButtle { get => _bossButtle; set => _bossButtle = value;}


    [SerializeField] private Fade _fade;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Boss _boss;
    [SerializeField] private SoundManager _soundManager;

    [SerializeField] private GameObject _gameSceneUI;
    [SerializeField] private GameObject _gameClearUI;
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private GameObject _bossUI;
    [SerializeField] private GameObject _abilitySelectUI;

    [SerializeField] private Button _backTitleButtonClear;
    [SerializeField] private Button _backTitleButtonGameOver;
    [SerializeField] private string _loadSceneName = "Opening";

    [SerializeField] private AudioSource _bgmAudioSource;
    [SerializeField] private AudioSource _seAudioSource;

    [Header("�n�܂�܂ł̑҂�����")]
    [SerializeField] private float _timeUntilStart;
    [Header("�n�܂��Ă���e�L�X�g��������܂ł̎���")]
    [SerializeField] private float _eraceTextTime;
    [Space(10)]
    [SerializeField] private TextMeshProUGUI _readyAndStartText;
    [Space(10)]

    System.Random _random = new System.Random();
    [Header("�G�̃v���n�u")] 
    [SerializeField] private GameObject[] _enemyList;
    [SerializeField] private  SpawnFourPoint _spawnPointClass;

    [Header("�{�X")]
    [SerializeField] private GameObject _enemyBoss;
    [Header("�������G���o������Ԋu")]
    [SerializeField] private int _spawnInterval = 2;
    [Header("�������G���ǂ̂��炢�|���Ǝ��̃t�F�[�Y�ɂȂ邩")]
    [SerializeField] private int _enemyNextPhaseNum = 20;

    [Header("animation���̃{�X")]
    [SerializeField] private GameObject _cinematicsBoss;
    [SerializeField] private Animator _bossAnimator;
    [SerializeField] private GameObject _playeraCam;
    [SerializeField] private GameObject _bossCam;

    private List<GameObject> _nowEnemyList = new List<GameObject>();//���݂̓G�L�����̐�

    [System.Serializable]
    public class AbilityUI
    {
        [Header("�\��UI")]
        public GameObject abilityUI;
        [Header("�o������G�L����")]
        public GameObject _enemy;
        [Header("Abillity��")]
        public Ability abilityType;
        [HideInInspector] public bool isSelected = false; // �I���ς݂��ǂ�����ێ�
    }

    [SerializeField] private List<AbilityUI> _abilityUIList;
    private int _currentAbilityListIndex = 0;

    [SerializeField] private GameObject _ability1Text;
    [SerializeField] private GameObject _ability2Text;

    [SerializeField] private Animator _uiAnimator;

    private static int _nowEnemyDefeatNum; //�G��|������
    private int _phase;
    private float _nowTime; //������

    private bool _gameScene;
    private bool _duringPlay = false; //�v���C���Ă����
    private bool _bossButtle = false;
    private bool _playMusic;

    private bool _choiseOne;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        _abilitySelectUI.SetActive(true);
        _ability1Text.SetActive(true);
        _gameClearUI.SetActive(false);
        _gameOverUI.SetActive(false);
        _nowEnemyDefeatNum = 0;

        _fade.FadeOut(1f);

        _phase = 0; //3�ɂ��邱�Ƃł����Ƀ{�X��ɂ͓���܂�

        if (_backTitleButtonClear!= null)
        {
            _backTitleButtonClear.onClick.AddListener(() => BackTitle());
        }
        if (_backTitleButtonGameOver != null)
        {
            _backTitleButtonGameOver.onClick.AddListener(() => BackTitle());
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (_gameScene)//�Q�[���V�[��
        {
            if (!_duringPlay) //�Q�[���J�n�O
            {
                if (_timeUntilStart >= _nowTime) //��[��
                {
                    _readyAndStartText.text = "Ready";
                    _nowTime += Time.deltaTime;
                }
                else //�X�^�[�g
                {
                    _readyAndStartText.text = "Start!!!";
                    StartCoroutine(WaitErace(_eraceTextTime));
                    _duringPlay = true;
                    _nowTime = 0;
                }
            }
            else //�Q�[�����s��
            {
                if (!_bossButtle)
                {
                    _nowTime += Time.deltaTime;
                    if (_nowTime > _spawnInterval)
                    {
                        if (_duringPlay)
                        {
                            HandlePhaseSpawns();
                            _nowTime = 0;
                        }
                        if (_phase == 3) //��3�t�F�[�Y�̎�
                        {
                            if(!(_nowEnemyList.Count > 0))//���ݓG�����Ȃ��ꍇ
                            {
                                _phase = 4;
                                StartCoroutine(BossAppearance());
                            }
                            else
                            {
                                HandlePhaseSpawns();
                                _nowTime = 0;
                            }
                        }
                    }
                }
            }
        }
        else//�A�r���e�B�I���V�[��
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ChangeAbillity(1);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ChangeAbillity(-1);
            }
            else if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                if (!_choiseOne)
                {
                    SelectOneAbillity();
                }
                else
                {
                    SelectSecondAbillity();
                }
            }
        }
    }
    /// <summary>
    /// ���݂̃t�F�[�Y�ɉ����ēG��I�u�W�F�N�g���X�|�[�����鏈��
    /// </summary>
    private void HandlePhaseSpawns()
    {
        switch (_phase)
        {
            case 0:
                SpawnAbilitySpecificEnemy(_playerController._firstAbilityType);
                break;
            case 1:
                SpawnRandomAbilitySpecificEnemy();
                break;
            case 2:
                SpawnFromEnemyList();
                break;
        }
    }
    /// <summary>
    /// ����̃A�r���e�B�^�C�v�ɑΉ�����G�𐶐��@Phase0
    /// </summary>
    /// <param name="abilityType">��������A�r���e�B�^�C�v</param>
    private void SpawnAbilitySpecificEnemy(Ability abilityType)
    {
        foreach (var abilityUI in _abilityUIList)
        {
            if (abilityUI.abilityType == abilityType)
            {
                Vector3 spawnPosition = GetRandomPositionInArea(_spawnPointClass.firstPoint, _spawnPointClass.secondPoint, _spawnPointClass.thirdPoint, _spawnPointClass.fourthPoint);
                GameObject enemy = Instantiate(abilityUI._enemy, spawnPosition, Quaternion.identity);
                Debug.Log(enemy);
                _nowEnemyList.Add(enemy);
                break;
            }
        }
    }
    /// <summary>
    /// �v���C���[�̏�������A�r���e�B�^�C�v�̂����ꂩ�ɑΉ�����G�������_���Ő����@Phase1
    /// </summary>
    private void SpawnRandomAbilitySpecificEnemy()
    {
        List<GameObject> spawnableEnemies = new List<GameObject>();
        foreach (var abilityUI in _abilityUIList)
        {
            if (abilityUI.abilityType == _playerController._firstAbilityType || abilityUI.abilityType == _playerController._secondAbilityType)
            {
                spawnableEnemies.Add(abilityUI._enemy);
            }
        }

        if (spawnableEnemies.Count > 0)
        {
            int randomIndex = _random.Next(0, spawnableEnemies.Count);
            Vector3 spawnPosition = GetRandomPositionInArea(_spawnPointClass.firstPoint, _spawnPointClass.secondPoint, _spawnPointClass.thirdPoint, _spawnPointClass.fourthPoint);
            GameObject enemy = Instantiate(spawnableEnemies[randomIndex], spawnPosition, Quaternion.identity);
            _nowEnemyList.Add(enemy);
        }
    }
    /// <summary>
    /// EnemyPrefabs �̒����烉���_���ɓG�𐶐�,Phase2
    /// </summary>
    private void SpawnFromEnemyList()
    {
        int randomIndex = _random.Next(0, _enemyList.Length);
        GameObject randomEnemy = _enemyList[randomIndex];

        if (randomEnemy != null)
        {
            Vector3 spawnPosition = GetRandomPositionInArea(_spawnPointClass.firstPoint, _spawnPointClass.secondPoint, _spawnPointClass.thirdPoint, _spawnPointClass.fourthPoint);
            GameObject enemy = Instantiate(randomEnemy, spawnPosition, Quaternion.identity);
            _nowEnemyList.Add(enemy);
        }
    }

    /// <summary>
    /// �\�͂̑I��UI��؂�ւ���
    /// </summary>
    /// <param name="direction">-1:����,1:�E��</param>
    private void ChangeAbillity(int direction)
    {
        // ���݂̔\��UI���\��
        _abilityUIList[_currentAbilityListIndex].abilityUI.SetActive(false);
        do
        {
            // �C���f�b�N�X���X�V
            _currentAbilityListIndex += direction;
            if (_currentAbilityListIndex < 0)
            {
                _currentAbilityListIndex = _abilityUIList.Count - 1;
            }
            else if (_currentAbilityListIndex >= _abilityUIList.Count)
            {
                _currentAbilityListIndex = 0;
            }
        }
        // �I���ς݂̔\�͂ł���΃X�L�b�v
        while (_abilityUIList[_currentAbilityListIndex].isSelected);

        // �V�����\��UI��\��
        _abilityUIList[_currentAbilityListIndex].abilityUI.SetActive(true);
    }
    /// <summary>
    /// ��ڂ̔\�͐ݒ�
    /// </summary>
    private void SelectOneAbillity()
    {
        _playerController._firstAbilityType = _abilityUIList[_currentAbilityListIndex].abilityType;
        _abilityUIList[_currentAbilityListIndex].isSelected = true;

        _abilityUIList[_currentAbilityListIndex].abilityUI.SetActive(false);

        ChangeAbillity(1);

        _playerController.SetAbility(
            _playerController._firstAbilityType,
            _playerController._firstAbilityImage,
            out _playerController.firstBallPrefabs,
            out _playerController.firstAbilityAudio,
            out _playerController.firstSpecialAbilityName
        );

        _ability1Text.SetActive(false);
        _ability2Text.SetActive(true);
        _choiseOne = true;
    }

    private void SelectSecondAbillity()
    {
        _playerController._secondAbilityType = _abilityUIList[_currentAbilityListIndex].abilityType;
        _abilityUIList[_currentAbilityListIndex].isSelected = true;

        _abilityUIList[_currentAbilityListIndex].abilityUI.SetActive(false);

        _playerController.SetAbility(
            _playerController._secondAbilityType,
            _playerController._secondAbilityImage,
            out _playerController.secondBallPrefabs,
            out _playerController.secondAbilityAudio,
            out _playerController.secondSpecialAbilityName
        );

        _ability2Text.SetActive(false);
        _abilitySelectUI.SetActive(true);
        _gameSceneUI.SetActive(true);
        _gameScene = true;
    }

    public void GameClear()
    {
        if (!_gameOverUI.activeSelf)
        {
            if (!_playMusic)
            {
                _duringPlay = false;
                _playerController.CanMove = false;
                PlaySE("�Q�[���N���A");
                _gameClearUI.SetActive(true);
                _playMusic = true;
                _playerController.PlayerAnimator.SetBool("Win", true);
            }
        }
    }

    public void GameOver()
    {
        if (!_gameClearUI.activeSelf)
        {
            if (!_playMusic)
            {
                _duringPlay = false;
                _playerController.CanMove = false;
                PlaySE("�Q�[���I�[�o�[");
                _gameOverUI.SetActive(true);
                _playMusic = true;
                _playerController.PlayerAnimator.SetBool("Lose", true);
            }
        }
    }
    /// <summary>
    /// �G�̎�ނ��������Ƃ��̏���
    /// </summary>
    public void IncreasedEnemyUI()
    {
        _uiAnimator.SetTrigger("Increased");
        PlaySE("�G�o���X�s�[�h�A�b�v");
    }
    /// <summary>
    /// �{�X�o��animation
    /// </summary>
    /// <returns></returns>
    public IEnumerator BossAppearance()
    {
        _uiAnimator.SetTrigger("Boss");
        _playerController.CanMove = false;
        _duringPlay = false;
        PlaySE("�{�X�o��");
        yield return new WaitForSeconds(4f);
        _bossCam.SetActive(true);
        yield return new WaitForSeconds(0.15f);
        _cinematicsBoss.SetActive(true);
        _gameSceneUI.SetActive(false);
        yield return new WaitForSeconds(2f);
        yield return new WaitForSeconds(2f);
        yield return new WaitForSeconds(2f);
        _uiAnimator.SetTrigger("BackGame");
        yield return new WaitForSeconds(1);
        _cinematicsBoss.SetActive(false);
        _bossCam.SetActive(false);
        _bossUI.SetActive(true);
        _playerController.CanMove = true;
        _bossButtle = true;
        _gameSceneUI.SetActive(true);
        _enemyBoss.SetActive(true);
        _boss.enabled = true;
        yield break;
    }
    //4�ӏ��͈̔͂̒����烉���_���ňʒu���擾
    Vector3 GetRandomPositionInArea(Transform t1, Transform t2,Transform t3,Transform t4)
    {
        float minX = Mathf.Min(t1.position.x, t2.position.x, t3.position.x, t4.position.x);
        float maxX = Mathf.Max(t1.position.x, t2.position.x, t3.position.x, t4.position.x);
        float minZ = Mathf.Min(t1.position.z, t2.position.z, t3.position.z, t4.position.z);
        float maxZ = Mathf.Max(t1.position.z, t2.position.z, t3.position.z, t4.position.z);

        float randomX = Random.Range(minX, maxX);
        float fixedY = -0.9f;
        float randomZ = Random.Range(minZ, maxZ);

        return new Vector3(randomX, fixedY, randomZ);
    }
    /// <summary>
    /// Ready,Start�Ŏ��Ԃ��o���������
    /// </summary>
    /// <param name="destroyTime"></param>
    /// <returns></returns>
    IEnumerator WaitErace(float destroyTime)
    {
        yield return new WaitForSeconds(destroyTime);
        _readyAndStartText.gameObject.SetActive(false);
    }
    /// <summary>
    /// SE���Q�b�g
    /// </summary>
    /// <param name="musicName"></param>
    /// <returns></returns>
    public AudioClip GetSE(string musicName)
    {
        foreach (SoundList soundList in _soundManager.soundLists)
        {
            if (soundList.soundName == musicName)
            {
                return soundList.audioClip;
            }
        }
        return null;
    }
    /// <summary>
    /// ���𗬂�
    /// </summary>
    /// <param name="seName"></param>
    public void PlaySE(string seName)
    {
        AudioClip se = GetSE(seName);
        if ((se != null && _seAudioSource != null))
        {
            _seAudioSource.PlayOneShot(se);
        }
    }
    /// <summary>
    /// �A�r���e�B�Őݒ肵���Ȃ𗬂�
    /// </summary>
    /// <param name="audio"></param>
    public void PlaySEAbility(AudioClip audio)
    {
        if(audio != null)
        {
            _seAudioSource.PlayOneShot(audio);
        }
    }
    /// <summary>
    /// �^�C�g���ɖ߂�
    /// </summary>
    private void BackTitle()
    {
        _fade.FadeIn(1f, () => SceneManager.LoadScene(_loadSceneName));
    }
}
