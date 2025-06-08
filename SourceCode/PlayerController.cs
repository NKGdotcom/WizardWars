using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static PlayerAbility;

public class PlayerController : MonoBehaviour//Player��HP�ƃX���C�_�[�̓����A����G�l�~�[���Ƃ�HP�ݒ�
    //��������� //�����邱�Ɓ@�AShift�����Ȃ���̍U���ōL�͈͂̍U���A�G���������Ƃ�����UI�̐ݒ�
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

    [SerializeField] private Transform _movementBoundsMin; // �ړ��\�G���A�̍ŏ��l
    [SerializeField] private Transform _movementBoundsMax; // �ړ��\�G���A�̍ő�l

    [SerializeField] private Animator _playerAnimator;

    [HideInInspector] public GameObject firstBallPrefabs;
    [HideInInspector] public GameObject secondBallPrefabs;

    [HideInInspector] public AudioClip firstAbilityAudio;
    [HideInInspector] public AudioClip secondAbilityAudio;

    [HideInInspector] public string firstSpecialAbilityName;
    [HideInInspector] public string secondSpecialAbilityName;

    [SerializeField] private float _specialMoveDuration; //�K�E�Z�̌��ʎ���(�ړ����x���x���Ȃ�Ƃ�)
    [SerializeField] private float _specialMoveCooldownFactor = 2f; //MP�񕜎��Ԃ̒x���{��
    [SerializeField] private float _baseSpecialMoveDuration = 3f;

    [Header("�e�����˂����ꏊ")]
    [SerializeField] private Transform _shotPoint;
    [Space(10)]
    [SerializeField] private Slider _playerHPSlider;
    [SerializeField] private Slider _playerMPSlider;
    [Space(10)]

    [HideInInspector] public Ability _firstAbilityType;
    [HideInInspector] public Ability _secondAbilityType;

    private float _playerHP, _playerMaxHP;
    private float _playerMP, _playerMaxMP;

    private float _notClickTime; //�N���b�N���Ă��牽�b��������
    private float _notClickInterval;

    private GameObject bullet;

    private bool _firstAbility;
    private bool _secondAbility;
    private bool _specialMove;

    private bool _canMove;

    private Vector3 _mousePosition;
    private Vector3 _previousPosition;

    private bool _onClick;//�N���b�N���Ă��邩���Ă��Ȃ���

 

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
                // �}�E�X�̈ʒu���擾���� (�n�ʂ��Ȃ��Ă���������悤�ɏC��)
                Plane plane = new Plane(Vector3.up, 0); // Y=0�̕���
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (plane.Raycast(ray, out float distance))
                {
                    _mousePosition = ray.GetPoint(distance); // ���ʂƂ̌�_���擾
                }
               
                    transform.position = Vector3.MoveTowards(transform.position, _mousePosition, _playerData.PlayerMoveSpeed * Time.deltaTime);//�ړ��̓���
                


                // �v���C���[�̃|�W�V�����������Ă��Ȃ�������
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
        if (direction.sqrMagnitude > 0.01f) // �����̌v�Z���[�������Ȃ��悤��
        {
            Quaternion mousePositionRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, mousePositionRotation, _playerData.PlayerRotationSpeed * Time.deltaTime);//��]����
        }
        if (_stageManager.DuringPlay)
        {
            if (_playerMP >= 1)
            {
                if (Input.GetMouseButtonDown(0))//��ڂ̔\�͂̋ʔ���
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
                if (Input.GetMouseButtonDown(1))//2�ڂ̔\�͂̋ʔ���
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
        if (!_onClick)//�N���b�N���Ă��Ȃ���
        {
            _notClickTime += Time.deltaTime;
            if (!_specialMove)
            {
                if (_notClickTime > _notClickInterval)//�C���^�[�o���ɒB������
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
            else //MP�̉񕜂��x��
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
    /// �K�E�Z�𔭓�����
    /// </summary>
    private void SpecialMoveActivation(string specialNameString)
    {
        _canMove = false;
        _specialMove = true;
        _specialMoveDuration = _baseSpecialMoveDuration + (_playerHP / _playerMaxMP) * 2f;
        _playerAnimator.SetBool("Run", false);
        //MP������
        _playerMP = 0;
        _playerMPSlider.value = _playerMP / _playerMaxMP;
        SpecialAbility(specialNameString);
        // �K�E�Z�̏I������
        StartCoroutine(SpecialMoveEndAfterDuration(_specialMoveDuration));
    }

    /// <summary>
    /// �K�E�Z���ʏI����̏���
    /// </summary>
    /// <param name="duration">���ʎ���</param>
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
    /// �K�E�Z�̖��O���擾���A�ǂ̕K�E�Z���g����悤�ɂ��邩
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
    /// 1�ڂ̔\�͂̋ʂ𔭎˂���
    /// </summary>
    void FirstAbilityShot()
    {
        bullet = Instantiate(firstBallPrefabs, _shotPoint.position, Quaternion.identity);
        _stageManager.PlaySEAbility(firstAbilityAudio);
        _playerAnimator.SetBool("MagicRelease", true);
        ShotAddForce();
    }

    /// <summary>
    /// 2�ڂ̔\�͂̋ʂ𔭎˂���
    /// </summary>
    void SecondAbilityShot()
    {
        bullet = Instantiate(secondBallPrefabs, _shotPoint.position, Quaternion.identity);
        _stageManager.PlaySEAbility(secondAbilityAudio);
        _playerAnimator.SetBool("MagicRelease", true);
        ShotAddForce();
    }

    /// <summary>
    ///�@�ʂɗ͂�������
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
    /// �\�͂�ς��� (���� �\�̖͂��O������)
    /// </summary>
    void ChangeAbility(string abilityName)
    {
        _stageManager.PlaySE("���@�̐؂�ւ�");
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
    /// �\�͂��Z�b�g
    /// </summary>
    /// <param name="ability">�\�̓^�C�v</param>
    /// <param name="abilityImage">�Z�b�g����摜</param>
    /// <param name="abilityBall"></param>
    public void SetAbility(Ability ability, Image abilityImage, out GameObject abilityBall, out AudioClip abilitySE, out string specialAbility)
    {
        KindOfAbility abilityData = _playerAbility.GetAbility(ability);

        if (abilityData != null)
        {
            abilityImage.sprite = abilityData.abilityImage;
            abilityBall = abilityData.bulletPurefabs;
            abilitySE = abilityData.abilitySE;
            specialAbility = abilityData.specialName; // �K�E�Z����ݒ�
        }
        else
        {
            Debug.LogWarning($"Ability {ability}��������܂���");
            abilityBall = null;
            abilitySE = null;
            specialAbility = null; // ������
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
                _stageManager.PlaySE("�_���[�W");
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
