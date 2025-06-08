using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static PlayerAbility;

public class PlayerController : MonoBehaviour//PlayerのHPとスライダーの同期、相手エネミーごとのHP設定
    //やったこと //今後やること　、Shift押しながらの攻撃で広範囲の攻撃、敵が増えたといったUIの設定
{
    public float PlayerHP
    {
        get => _playerHP;
        set
        {
            _playerHP = value;
            if (_playerHPSlider != null)
            {
                _playerHPSlider.value = _playerHP/_playerMaxHP;
            }
        }
    }

    public bool CanMove { get => _canMove; set => _canMove = value; }
    public Animator PlayerAnimator { get => _playerAnimator; set => _playerAnimator = value; }

    [SerializeField] private PlayerData _playerData;
    [SerializeField] private PlayerAbility _playerAbility;
    [SerializeField] private StageManager _stageManager;
    [SerializeField] private SoundManager _soundManager;

    [SerializeField] private FireSpecialMove _fireSpecialMove;
    [SerializeField] private IceSpecialMove _iceSpecialMove;
    [SerializeField] private ThunderSpecialMove _thunderSpecialMove;

    [SerializeField] private GameObject _camera;
    private Vector3 _cameraOffset;

    [SerializeField] public Image _firstAbilityImage;
    [SerializeField] public Image _secondAbilityImage;

    [SerializeField] private Transform _movementBoundsMin; // 移動可能エリアの最小値
    [SerializeField] private Transform _movementBoundsMax; // 移動可能エリアの最大値

    [SerializeField] private Animator _playerAnimator;

    [HideInInspector] public GameObject firstBallPrefabs;
    [HideInInspector] public GameObject secondBallPrefabs;

    [HideInInspector] public AudioClip firstAbilityAudio;
    [HideInInspector] public AudioClip secondAbilityAudio;

    [HideInInspector] public string firstSpecialAbilityName;
    [HideInInspector] public string secondSpecialAbilityName;

    [SerializeField] private float _specialMoveDuration; //必殺技の効果時間(移動速度が遅くなるとか)
    [SerializeField] private float _specialMoveCooldownFactor = 2f; //MP回復時間の遅延倍率
    [SerializeField] private float _baseSpecialMoveDuration = 3f;

    [Header("弾が発射される場所")]
    [SerializeField] private Transform _shotPoint;
    [Space(10)]
    [SerializeField] private Slider _playerHPSlider;
    [SerializeField] private Slider _playerMPSlider;
    [Space(10)]

    [HideInInspector] public Ability _firstAbilityType;
    [HideInInspector] public Ability _secondAbilityType;

    private float _playerHP, _playerMaxHP;
    private float _playerMP, _playerMaxMP;

    private float _notClickTime; //クリックしてから何秒たったか
    private float _notClickInterval;

    private GameObject bullet;

    private bool _firstAbility;
    private bool _secondAbility;
    private bool _specialMove;

    private bool _canMove;

    private Vector3 _mousePosition;
    private Vector3 _previousPosition;

    private bool _onClick;//クリックしているかしていないか

 

    private void Awake()
    {
        
    }
    void Start()
    {
        _cameraOffset = _camera.transform.position - transform.position;
        _stageManager.DuringPlay= false;
        _secondAbility = false;
        _firstAbilityImage.enabled = true;
        _secondAbilityImage.enabled = false;

        _firstAbility = true;
        _secondAbility = false;

        _canMove = true;

        _playerHP = _playerMaxHP = _playerData.PlayerHP;
        _playerMP = _playerMaxMP = _playerData.PlayerMP;

        _playerHPSlider.value = 1;
        _playerMPSlider.value = 1;

        _notClickInterval = _playerData.BulletInterval;
    }

    void Update()
    {

        _camera.transform.position = transform.position + _cameraOffset;
        if (_stageManager.DuringPlay)
        {
            if (_canMove)
            {
                // マウスの位置を取得する (地面がなくても動かせるように修正)
                Plane plane = new Plane(Vector3.up, 0); // Y=0の平面
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (plane.Raycast(ray, out float distance))
                {
                    _mousePosition = ray.GetPoint(distance); // 平面との交点を取得
                }
               
                    transform.position = Vector3.MoveTowards(transform.position, _mousePosition, _playerData.PlayerMoveSpeed * Time.deltaTime);//移動の動作
                


                // プレイヤーのポジションが動いていなかったら
                if (Vector3.Distance(transform.position, _previousPosition) < 0.001f)
                {
                    _playerAnimator.SetBool("Run", false);
                }
                else
                {
                    _playerAnimator.SetBool("Run", true);
                }

                _previousPosition = transform.position;
            }
        }

        Vector3 direction = _mousePosition - transform.position;
        if (direction.sqrMagnitude > 0.01f) // 向きの計算がゼロ割しないように
        {
            Quaternion mousePositionRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, mousePositionRotation, _playerData.PlayerRotationSpeed * Time.deltaTime);//回転動作
        }
        if (_stageManager.DuringPlay)
        {
            if (_playerMP >= 1)
            {
                if (Input.GetMouseButtonDown(0))//一つ目の能力の玉発射
                {
                    if (_secondAbility)
                    {
                        ChangeAbility("First");
                    }
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        SpecialMoveActivation(firstSpecialAbilityName);
                        Debug.Log(firstSpecialAbilityName);
                        return;
                    }
                    _playerAnimator.SetBool("MagicRelease", false);
                    _onClick = true;
                    if (_firstAbility)
                    {
                        FirstAbilityShot();
                    }
                }
                if (Input.GetMouseButtonDown(1))//2つ目の能力の玉発射
                {
                    if (_firstAbility)
                    {
                        ChangeAbility("Second");
                    }
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        SpecialMoveActivation(secondSpecialAbilityName);
                        Debug.Log(secondSpecialAbilityName);
                        return;
                    }
                    _playerAnimator.SetBool("MagicRelease", false);
                    _onClick = true;
                    if (_secondAbility)
                    {
                        SecondAbilityShot();
                    }
                }
            }
            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            {
                _onClick = false;
            }
        }
        if (!_onClick)//クリックしていない間
        {
            _notClickTime += Time.deltaTime;
            if (!_specialMove)
            {
                if (_notClickTime > _notClickInterval)//インターバルに達したら
                {
                    _playerMP += 1;
                    _playerMPSlider.value = _playerMP / _playerMaxMP;
                    if (_playerMP >= _playerMaxMP)
                    {
                        _playerMP = _playerMaxMP;
                        _notClickTime = 0;
                    }
                }
            }
            else //MPの回復が遅い
            {
                if (_notClickTime > _notClickInterval * _specialMoveCooldownFactor)
                {
                    _playerMP += 1;
                    _playerMPSlider.value = _playerMP / _playerMaxMP;
                    if (_playerMP >= _playerMaxMP)
                    {
                        _playerMP = _playerMaxMP;
                        _notClickTime = 0;
                    }
                }
            }
        }
        else 
        { 
            _notClickTime = 0; 
        }
    }
    /// <summary>
    /// 必殺技を発動する
    /// </summary>
    private void SpecialMoveActivation(string specialNameString)
    {
        _canMove = false;
        _specialMove = true;
        _specialMoveDuration = _baseSpecialMoveDuration + (_playerHP / _playerMaxMP) * 2f;
        _playerAnimator.SetBool("Run", false);
        //MPを消費
        _playerMP = 0;
        _playerMPSlider.value = _playerMP / _playerMaxMP;
        SpecialAbility(specialNameString);
        // 必殺技の終了処理
        StartCoroutine(SpecialMoveEndAfterDuration(_specialMoveDuration));
    }

    /// <summary>
    /// 必殺技効果終了後の処理
    /// </summary>
    /// <param name="duration">効果時間</param>
    IEnumerator SpecialMoveEndAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        _playerAnimator.SetBool("Run", true);
        _specialMove = false;
        _canMove = true;
        _fireSpecialMove.enabled = false;
        _iceSpecialMove.enabled = false;
        _thunderSpecialMove.enabled = false;
    }
    /// <summary>
    /// 必殺技の名前を取得し、どの必殺技を使えるようにするか
    /// </summary>
    /// <param name="specialName"></param>
    private void SpecialAbility(string specialName)
    {
        switch(specialName)
        {
            case "SpecialMoveFire":
                _fireSpecialMove.enabled = true;
                break;
            case "SpecialMoveIce":
                _iceSpecialMove.enabled= true;
                break;
            case "SpecialMoveThunder":
                _thunderSpecialMove.enabled= true;
                break;
        }
    }

    /// <summary>
    /// 1つ目の能力の玉を発射する
    /// </summary>
    void FirstAbilityShot()
    {
        bullet = Instantiate(firstBallPrefabs, _shotPoint.position, Quaternion.identity);
        _stageManager.PlaySEAbility(firstAbilityAudio);
        _playerAnimator.SetBool("MagicRelease", true);
        ShotAddForce();
    }

    /// <summary>
    /// 2つ目の能力の玉を発射する
    /// </summary>
    void SecondAbilityShot()
    {
        bullet = Instantiate(secondBallPrefabs, _shotPoint.position, Quaternion.identity);
        _stageManager.PlaySEAbility(secondAbilityAudio);
        _playerAnimator.SetBool("MagicRelease", true);
        ShotAddForce();
    }

    /// <summary>
    ///　玉に力を加える
    /// </summary>
    void ShotAddForce()
    {
        _playerMP--;
        _playerMPSlider.value = _playerMP / _playerMaxMP;
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(_shotPoint.forward * _playerData.ButtletSpeed);
        Destroy(bullet, 3);
        StartCoroutine(ResetBoolAfterDelay("MagicRelease", 0.5f));
    }
    /// <summary>
    /// 能力を変える (引数 能力の名前を入れる)
    /// </summary>
    void ChangeAbility(string abilityName)
    {
        _stageManager.PlaySE("魔法の切り替え");
        switch (abilityName)
        {
            case "First":
                _firstAbility = true;
                _firstAbilityImage.enabled = true;
                _secondAbilityImage.enabled = false;
                _secondAbility = false;
                break;
            case "Second":
                _secondAbility = true;
                _secondAbilityImage.enabled = true;
                _firstAbilityImage.enabled = false;
                _firstAbility = false;
                break;
        }
    }
    /// <summary>
    /// 能力をセット
    /// </summary>
    /// <param name="ability">能力タイプ</param>
    /// <param name="abilityImage">セットする画像</param>
    /// <param name="abilityBall"></param>
    public void SetAbility(Ability ability, Image abilityImage, out GameObject abilityBall, out AudioClip abilitySE, out string specialAbility)
    {
        KindOfAbility abilityData = _playerAbility.GetAbility(ability);

        if (abilityData != null)
        {
            abilityImage.sprite = abilityData.abilityImage;
            abilityBall = abilityData.bulletPurefabs;
            abilitySE = abilityData.abilitySE;
            specialAbility = abilityData.specialName; // 必殺技名を設定
        }
        else
        {
            Debug.LogWarning($"Ability {ability}が見つかりません");
            abilityBall = null;
            abilitySE = null;
            specialAbility = null; // 初期化
        }
    }
    IEnumerator ResetBoolAfterDelay(string param, float delay)
    {
        yield return new WaitForSeconds(delay);
        _playerAnimator.SetBool(param, false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ice") || other.gameObject.CompareTag("Fire")|| other.gameObject.CompareTag("Thunder") || other.gameObject.CompareTag("Rock"))
        {
            if (_playerHP > 0)
            {
                _stageManager.PlaySE("ダメージ");
                _playerHP--;
                _playerHPSlider.value = _playerHP / _playerMaxHP;
                _playerAnimator.SetBool("GetDamage", true);
                Destroy(other.gameObject);
                StartCoroutine(ResetBoolAfterDelay("GetDamage", 0.5f));

                _stageManager.NowEnemyList.Remove(other.gameObject);
            }
        }
        else if (other.gameObject.CompareTag("Boss"))
        {
            _playerHP -= 2;
            _playerHPSlider.value = _playerHP / _playerMaxHP;
        }
        if (_playerHP <= 0)
        {
            _stageManager.GameOver();
            _canMove = false;
            _stageManager.DuringPlay = false;
        }
    }
}
