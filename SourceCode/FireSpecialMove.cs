using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerAbility;
using static StageManager;

public class FireSpecialMove : MonoBehaviour//�傫�ȉ΂��o��������B�����Đ��b��HP�_�E��������������
{
    [SerializeField] private PlayerAbility _playerAbility;
    [SerializeField] private Ability _fireAbility;
    [SerializeField] private Boss _boss;

    [SerializeField] private GameObject _fireBallPrefabs;
    private AudioClip _fireAbilityAudio;
    [SerializeField] private int _fireBallScale;

    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _instantiateBallPosition;

    [SerializeField] private float _accumulateTime; //���ߎ���
    [SerializeField] private float _fireDuration; //���ǎ�������
    private float _nowAccumulateTime;
    private float _nowFireDuration;

    private Vector3 _initialScale;

    // Start is called before the first frame update
    void OnEnable()
    {
        SetAbillity(_fireAbility,_fireAbilityAudio);
        //�傫�ȉ΂̋ʐ���

        _fireBallPrefabs.SetActive(true);
        _fireBallPrefabs.transform.position = _instantiateBallPosition.position;
        _fireBallPrefabs.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
        _initialScale = _fireBallPrefabs.transform.localScale;
        _fireBallPrefabs.transform.localScale = Vector3.zero;

        StartCoroutine(ScaleUpCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        _nowAccumulateTime += Time.deltaTime;
        if(_nowAccumulateTime >= _accumulateTime)
        {
            _nowAccumulateTime = 0;

            FireProjectile();
        }
    }
    /// <summary>
    /// �A�r���e�B���Z�b�g
    /// </summary>
    /// <param name="ability"></param>
    /// <param name="prefabs"></param>
    /// <param name="abilityAudio"></param>
    private void SetAbillity(Ability ability, AudioClip abilityAudio)
    {
        KindOfAbility abilityData = _playerAbility.GetAbility(ability);
        if (abilityData != null)
        {
            abilityAudio = abilityData.abilitySE;
        }
        else
        {
            Debug.LogWarning($"Ability {ability}��������܂���");
        }
    }
    /// <summary>
    /// �΂̋ʂ𔭎�
    /// </summary>
    private void FireProjectile()
    {
        if(_fireBallPrefabs != null)
        {
            if (_fireBallPrefabs != null)
            {
                Rigidbody rb = _fireBallPrefabs.GetComponent<Rigidbody>();

                Vector3 fireDirection = (_instantiateBallPosition.forward + -_instantiateBallPosition.up).normalized;
                rb.velocity = fireDirection * 10f;
            }
        }
    }
    private IEnumerator ScaleUpCoroutine()
    {
        float scaleDuration = _accumulateTime;
        float timer = 0;

        while (timer < scaleDuration)
        {
            timer += Time.deltaTime;
            float scaleProgress = timer / scaleDuration;
            _fireBallPrefabs.transform.localScale = Vector3.Lerp(Vector3.zero, _initialScale * _fireBallScale, scaleProgress);
            yield return null;
        }
    }
}
