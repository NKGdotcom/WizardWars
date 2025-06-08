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
    [Header("スポーンさせる範囲を4角形として囲う")]
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

    [Header("始まるまでの待ち時間")]
    [SerializeField] private float _timeUntilStart;
    [Header("始まってからテキストが消えるまでの時間")]
    [SerializeField] private float _eraceTextTime;
    [Space(10)]
    [SerializeField] private TextMeshProUGUI _readyAndStartText;
    [Space(10)]

    System.Random _random = new System.Random();
    [Header("敵のプレハブ")] 
    [SerializeField] private GameObject[] _enemyList;
    [SerializeField] private  SpawnFourPoint _spawnPointClass;

    [Header("ボス")]
    [SerializeField] private GameObject _enemyBoss;
    [Header("小さい敵が出現する間隔")]
    [SerializeField] private int _spawnInterval = 2;
    [Header("小さい敵をどのくらい倒すと次のフェーズになるか")]
    [SerializeField] private int _enemyNextPhaseNum = 20;

    [Header("animation時のボス")]
    [SerializeField] private GameObject _cinematicsBoss;
    [SerializeField] private Animator _bossAnimator;
    [SerializeField] private GameObject _playeraCam;
    [SerializeField] private GameObject _bossCam;

    private List<GameObject> _nowEnemyList = new List<GameObject>();//現在の敵キャラの数

    [System.Serializable]
    public class AbilityUI
    {
        [Header("能力UI")]
        public GameObject abilityUI;
        [Header("出現する敵キャラ")]
        public GameObject _enemy;
        [Header("Abillity名")]
        public Ability abilityType;
        [HideInInspector] public bool isSelected = false; // 選択済みかどうかを保持
    }

    [SerializeField] private List<AbilityUI> _abilityUIList;
    private int _currentAbilityListIndex = 0;

    [SerializeField] private GameObject _ability1Text;
    [SerializeField] private GameObject _ability2Text;

    [SerializeField] private Animator _uiAnimator;

    private static int _nowEnemyDefeatNum; //敵を倒した数
    private int _phase;
    private float _nowTime; //現時間

    private bool _gameScene;
    private bool _duringPlay = false; //プレイしている間
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

        _phase = 0; //3にすることですぐにボス戦には入れます

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
        if (_gameScene)//ゲームシーン
        {
            if (!_duringPlay) //ゲーム開始前
            {
                if (_timeUntilStart >= _nowTime) //よーい
                {
                    _readyAndStartText.text = "Ready";
                    _nowTime += Time.deltaTime;
                }
                else //スタート
                {
                    _readyAndStartText.text = "Start!!!";
                    StartCoroutine(WaitErace(_eraceTextTime));
                    _duringPlay = true;
                    _nowTime = 0;
                }
            }
            else //ゲーム実行中
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
                        if (_phase == 3) //第3フェーズの時
                        {
                            if(!(_nowEnemyList.Count > 0))//現在敵がいない場合
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
        else//アビリティ選択シーン
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
    /// 現在のフェーズに応じて敵やオブジェクトをスポーンする処理
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
    /// 特定のアビリティタイプに対応する敵を生成　Phase0
    /// </summary>
    /// <param name="abilityType">生成するアビリティタイプ</param>
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
    /// プレイヤーの所持するアビリティタイプのいずれかに対応する敵をランダムで生成　Phase1
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
    /// EnemyPrefabs の中からランダムに敵を生成,Phase2
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
    /// 能力の選択UIを切り替える
    /// </summary>
    /// <param name="direction">-1:左へ,1:右へ</param>
    private void ChangeAbillity(int direction)
    {
        // 現在の能力UIを非表示
        _abilityUIList[_currentAbilityListIndex].abilityUI.SetActive(false);
        do
        {
            // インデックスを更新
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
        // 選択済みの能力であればスキップ
        while (_abilityUIList[_currentAbilityListIndex].isSelected);

        // 新しい能力UIを表示
        _abilityUIList[_currentAbilityListIndex].abilityUI.SetActive(true);
    }
    /// <summary>
    /// 一つ目の能力設定
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
                PlaySE("ゲームクリア");
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
                PlaySE("ゲームオーバー");
                _gameOverUI.SetActive(true);
                _playMusic = true;
                _playerController.PlayerAnimator.SetBool("Lose", true);
            }
        }
    }
    /// <summary>
    /// 敵の種類が増えたときの処理
    /// </summary>
    public void IncreasedEnemyUI()
    {
        _uiAnimator.SetTrigger("Increased");
        PlaySE("敵出現スピードアップ");
    }
    /// <summary>
    /// ボス出現animation
    /// </summary>
    /// <returns></returns>
    public IEnumerator BossAppearance()
    {
        _uiAnimator.SetTrigger("Boss");
        _playerController.CanMove = false;
        _duringPlay = false;
        PlaySE("ボス出現");
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
    //4箇所の範囲の中からランダムで位置を取得
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
    /// Ready,Startで時間が経ったら消去
    /// </summary>
    /// <param name="destroyTime"></param>
    /// <returns></returns>
    IEnumerator WaitErace(float destroyTime)
    {
        yield return new WaitForSeconds(destroyTime);
        _readyAndStartText.gameObject.SetActive(false);
    }
    /// <summary>
    /// SEをゲット
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
    /// 音を流す
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
    /// アビリティで設定した曲を流す
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
    /// タイトルに戻す
    /// </summary>
    private void BackTitle()
    {
        _fade.FadeIn(1f, () => SceneManager.LoadScene(_loadSceneName));
    }
}
