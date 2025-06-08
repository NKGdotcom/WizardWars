using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;


public class Boss : MonoBehaviour
{
    [SerializeField] private StageManager _stageManager;

    [SerializeField] private float _bossHP;
    //[SerializeField] private int _bossHPPhase1;
    //[SerializeField] private int _bossHPPhase2;
    private float _bossMaxHP;

    [Header("火の必殺")]
    [SerializeField] private float _fireDuration;
    [SerializeField] private float _fireDelayDestroy;
    [SerializeField] private GameObject _fireEffect;
    [Header("氷の必殺")]
    [SerializeField] private float _slowDuration;
    [SerializeField] private float _slowFactor;
    [SerializeField] private GameObject _debuffEffect;
    private bool _ice;

    [Header("以下の攻撃たちの確率とインターバルを設定")]
    [SerializeField] private int _bossAttackPanchProbability = 40; //パンチ攻撃確率
    [SerializeField] private float _bossAttackPanchInterval; //パンチ後のインターバル
    [SerializeField] private GameObject _panchObject;
    [SerializeField] private GameObject _panchCollision;
    [SerializeField] private GameObject _rockParticleObj;

    [SerializeField] private int _bossAttackTramplingProbability = 20; //ジャンプ攻撃確率
    [SerializeField] private float _bossAttackTramplingInterval; //ジャンプ攻撃後のインターバル
    [SerializeField] private GameObject _jumpAttackCollision;
    [SerializeField] private GameObject _rockAttack;

    [SerializeField] private int _bossAttackThrowProbability = 10; //岩を飛ばす確率
    [SerializeField] private float _bossAttackThrowInterval; //岩を飛ばす攻撃後のインターバル
    [SerializeField] private GameObject _rockThrowObj; //岩を投げるアニメーション
    [SerializeField] private GameObject[] _rockList;
    [SerializeField] private Transform _rockInstantiatePoint;
    [SerializeField] private Transform _rockInstantiatePoint2;

    [SerializeField] private int _bossAttackSummoningProbability = 5; //モンスターを召喚する確率
    [SerializeField] private float _bossAttackSummoningInterval; //モンスター召喚のインターバル
    [SerializeField] private GameObject[] _monsterKind;//モンスターの種類

    [SerializeField] private int _summoningNum; //モンスターを一度に召喚する数
    [SerializeField] private Transform _instantiateMonsterPos;
    [SerializeField] private Transform _instantiateMonsterPos2;
    [SerializeField] private float _instantiateHeight;
    private int _probabilitySum;

    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _approachSpeed = 2.0f; //ボスの移動速度
    [SerializeField] private float _attackRange = 5.0f; //ボスが攻撃を開始する範囲

    [SerializeField] private Animator _bossAnimator;

    [SerializeField] private Slider _bossHPSlider;

    private bool _normal; //通常時
    private bool _move; //移動中
    private bool _attack; //攻撃中
    private bool _hit;
    private bool _dead; //死んだ

    // Start is called before the first frame update
    void Start()
    {
        _probabilitySum = ProbabilitySum();

        _normal = true;
        _move = false;
        _attack = false;
        _dead = false;
        _hit = false;

        StartCoroutine(StateMachine());

        _bossMaxHP = _bossHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (_move && !_attack && !_dead)
        {
            MoveTowardsPlayer();
        }
    }
    /// <summary>
    /// 合計の確率
    /// </summary>
    /// <returns></returns>
    private int ProbabilitySum()
    {
        return _bossAttackPanchProbability +
               _bossAttackTramplingProbability +
               _bossAttackThrowProbability +
               _bossAttackSummoningProbability;
    }
    /// <summary>
    /// 状態遷移のコルーチン
    /// </summary>
    private IEnumerator StateMachine()
    {
        _bossAnimator.SetTrigger("Idle");
        yield return new WaitForSeconds(2.0f);

        _normal = false;
        _move = true;

        while (!_dead)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);

            if (distanceToPlayer <= _attackRange && !_attack)
            {
                // プレイヤーが攻撃範囲内に入ったら攻撃を開始
                //Debug.Log("攻撃");
                _move = false;
                StartCoroutine(AttackRoutine());
            }
            else if (distanceToPlayer > _attackRange && !_attack)
            {
              
            }

            yield return null;
        }
    }
    /// <summary>
    /// プレイヤーに向かって移動する
    /// </summary>
    private void MoveTowardsPlayer()
    {
        Vector3 direction = (_playerTransform.position - transform.position).normalized;
        
        transform.position += direction * _approachSpeed * Time.deltaTime;
        transform.LookAt(_playerTransform.transform);

        // 移動アニメーションを再生
        _bossAnimator.SetTrigger("Move");
    }
    private IEnumerator AttackRoutine()
    {
        if (!_dead && !_normal && !_attack)
        {
            int randomValue = Random.Range(1, _probabilitySum + 1);
            if (randomValue <= _bossAttackPanchProbability)
            {
                yield return ExecuteAttack("Panch", _bossAttackPanchInterval);
                StartCoroutine(Panch());
            }
            else if (randomValue <= _bossAttackPanchProbability + _bossAttackTramplingProbability)
            {
                yield return ExecuteAttack("Trampling", _bossAttackTramplingInterval);
                StartCoroutine(TrampringAttack());
            }
            else if (randomValue <= _bossAttackPanchProbability + _bossAttackTramplingProbability + _bossAttackThrowProbability)
            {
                yield return ExecuteAttack("TramplingThrow", _bossAttackThrowInterval);
                StartCoroutine(ThrowRock());
            }
            else
            {
                yield return ExecuteAttack("Summoning", _bossAttackSummoningInterval);
                StartCoroutine(Summoning());
            }
        }
    }
    /// <summary>
    /// 実際の攻撃を実行し、インターバルを設定する
    /// </summary>
    /// <param name="attackType">攻撃の種類</param>
    /// <param name="interval">インターバル時間</param>
    private IEnumerator ExecuteAttack(string attackType, float interval)
    {
        _attack = true;
        Debug.Log($"Executing attack: {attackType}");
        _bossAnimator.SetTrigger(attackType);
        yield break;
    }
    /// <summary>
    /// パンチ攻撃
    /// </summary>
    /// <returns></returns>
    private IEnumerator Panch()
    {
        ParticleSystem rockParticle = _rockParticleObj.GetComponent<ParticleSystem>();
        if (_hit == true)
        {
            _attack = false;
            _move = true;
            yield break;
        }
        _panchObject.SetActive(true);
        yield return new WaitForSeconds(1);
        if(_hit == true) 
        {
            _attack = false;
            _move = true;
            _panchObject.SetActive(false);
            yield break;
        }
        rockParticle.Play();
        _stageManager.PlaySE("パンチ");
        _panchCollision.SetActive(true);
        _rockParticleObj.SetActive(true);
        yield return new WaitForSeconds(2);

        _panchCollision.SetActive(false);
        _panchObject.SetActive(false);
        _rockParticleObj.SetActive(false);
        rockParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        _attack = false;
        _move = true;
        yield break;
    }
    /// <summary>
    /// ジャンプ攻撃
    /// </summary>
    /// <returns></returns>
    private IEnumerator TrampringAttack()
    {
        if (_hit == true)
        {
            _attack = false;
            _move = true;
            yield break;
        }
        yield return new WaitForSeconds(1.75f);
        if (_hit == true)
        {
            _attack = false;
            _move = true;
            yield break;
        }
        _jumpAttackCollision.SetActive(true);
        _stageManager.PlaySE("ジャンプ攻撃");
        _rockAttack.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        _jumpAttackCollision.SetActive(false);
        _rockAttack.SetActive(false);
        _attack = false;
        _move = true;
        yield break;
    }
    /// <summary>
    /// 岩を投げる演出
    /// </summary>
    /// <returns></returns>
    private IEnumerator ThrowRock()
    {
        if (_hit == true)
        {
            _attack = false;
            _move = true;
            yield break;
        }
        yield return new WaitForSeconds(0.5f);
        _rockThrowObj.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        if (_hit == true)
        {
            _attack = false;
            _move = true;
            _rockThrowObj.SetActive(false);
            yield break;
        }
        InstantiateRock();
        _stageManager.PlaySE("岩");
        _attack = false;
        _move = true;
        yield return new WaitForSeconds(5);
        _rockThrowObj.SetActive(false);
        yield break;
    }
    /// <summary>
    /// 岩を出現
    /// </summary>
    private void InstantiateRock()
    {
        for(int  i = 0; i < _rockList.Length; i++)
        {
            float randomX = Random.Range(_rockInstantiatePoint.position.x, _rockInstantiatePoint2.position.x);
            float randomY = Random.Range(_rockInstantiatePoint.position.y - 15, _rockInstantiatePoint.position.y + 15);
            float randomZ = Random.Range(_rockInstantiatePoint.position.z, _rockInstantiatePoint2.position.z);

            Vector3 spawnPosition = new Vector3(randomX, randomY, randomZ);
            _rockList[i].SetActive(true);
            _rockList[i].gameObject.transform.position = spawnPosition;
        }
    }
    /// <summary>
    /// 敵を召喚
    /// </summary>
    /// <returns></returns>
    private IEnumerator Summoning()
    {
        yield return new WaitForSeconds(0.65f);
        for (int i = 0; i < _summoningNum; i++)
        {
            // ランダムな位置を計算
            float randomX = Random.Range(_instantiateMonsterPos.position.x, _instantiateMonsterPos2.position.x);
            float randomZ = Random.Range(_instantiateMonsterPos.position.z, _instantiateMonsterPos2.position.z);
            float fixedY =  _instantiateHeight; 
            Vector3 spawnPosition = new Vector3(randomX, fixedY, randomZ);

            int spawnIndex = Random.Range(0,_monsterKind.Length);
            // オブジェクトを生成
            Instantiate(_monsterKind[spawnIndex], spawnPosition, Quaternion.identity);
        }
        _attack = false;
        _move = true;
        yield break;
    }
    /// <summary>
    /// ボスが倒されて、ゲームクリア
    /// </summary>
    /// <returns></returns>
    private IEnumerator DieBoss()
    {
        _dead = true;
        _bossAnimator.SetTrigger("Die");
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y - 4, startPosition.z);

        float duration = 3.0f; // 3秒かけて移動
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            // 徐々に位置を変更する
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime; // 経過時間を加算
            yield return null; // フレームを待つ
        }
        transform.position = endPosition;

        _stageManager.GameClear();
        yield break;
    }
    /// <summary>
    /// ダメージを持続させる
    /// </summary>
    /// <returns></returns>
    private IEnumerator ApplyDamageOverTime()
    {
        float elapsedTime = 0;
        while(elapsedTime < _fireDuration)
        {
            _fireEffect.SetActive(true);
            elapsedTime += Time.deltaTime;
            _bossHP-= Time.deltaTime ;//valueが適用されていない
            _bossHPSlider.value = _bossHP / _bossMaxHP;
            if(elapsedTime >= _fireDuration)
            {
                _fireEffect.SetActive(false);
            }
            yield return null;
        }
        Debug.Log("finish");
        if(_bossHP <= 0)
        {
            _stageManager.GameClear();
        }
    }
    /// <summary>
    /// ボスのスピードが下がる
    /// </summary>
    /// <returns></returns>
    private IEnumerator SlowBoss()
    {
        if (!_ice)
        {
            _ice = true;
            _debuffEffect.SetActive(true);
            float initialSpeed = _approachSpeed;
            _approachSpeed = _slowFactor;
            yield return new WaitForSeconds(_slowDuration);
            _debuffEffect.SetActive(false);
            _approachSpeed = initialSpeed;
            _ice = false;
            yield break;
        }
    }
    private IEnumerator BoolHit()
    {
        yield return new WaitForSeconds(1.2f);
        _hit = false;
        yield break;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FireBall")|| collision.gameObject.CompareTag("IceBall") || collision.gameObject.CompareTag("ThunderBall"))
        {
            _hit = true;
            _stageManager.PlaySE("ダメージ");
            _bossAnimator.SetTrigger("Hit");
            _bossHP--;
            _bossHPSlider.value = _bossHP / _bossMaxHP;
            Destroy(collision.gameObject);
            StartCoroutine(BoolHit());
            if (_bossHP <= 0)
            {
                StartCoroutine(DieBoss());
            }
        }
        else if (collision.gameObject.CompareTag("BigFire"))
        {
            _hit = true;
            _stageManager.PlaySE("炎");
            _bossHP--;
            _bossHPSlider.value = _bossHP / _bossMaxHP;
            collision.gameObject.SetActive(false);
            StartCoroutine(ApplyDamageOverTime());
            StartCoroutine(BoolHit());
            if (_bossHP <= 0)
            {
                StartCoroutine(DieBoss());
            }
        }
        else if (collision.gameObject.CompareTag("BigIce"))
        {
            _hit = true;
            _stageManager.PlaySE("氷");
            _bossHP--;
            _bossHPSlider.value = _bossHP / _bossMaxHP;
            collision.gameObject.SetActive(false);
            StartCoroutine(SlowBoss());
            StartCoroutine(BoolHit());
            if (_bossHP <= 0)
            {
                StartCoroutine(DieBoss());
            }
        }
        else if (collision.gameObject.CompareTag("BigThunder"))
        {
            _hit = true;
            _stageManager.PlaySE("雷");
            _bossHP--;
            _bossHPSlider.value = _bossHP / _bossMaxHP;
            collision.gameObject.SetActive(false);
            StartCoroutine(BoolHit());
            if (_bossHP <= 0)
            {
                StartCoroutine(DieBoss());
            }
        }
    }
}

