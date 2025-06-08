using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static StageManager;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private Ability _enemyAbility;

    private StageManager _stageManager;
    private PlayerController _playerController;

    private float _moveSpeed;
    private int _enemyHP;
    private string _tagName;
    private GameObject _targetPlayer;

    // Start is called before the first frame update
    void Start()
    {
        GameObject stageManager = GameObject.FindWithTag("StageManager");
        _stageManager = stageManager.GetComponent<StageManager>();
        _targetPlayer = GameObject.FindGameObjectWithTag("Player");
        _playerController = _targetPlayer.GetComponent<PlayerController>();

        SetParameter(_enemyAbility);
    }

    // Update is called once per frame
    void Update()
    {
        ChasePlayer();
    }
    /// <summary>
    /// プレイヤーを追いかける
    /// </summary>
    private void ChasePlayer()
    {
        transform.LookAt(_targetPlayer.transform);
        transform.position += transform.forward * _moveSpeed;
    }
    /// <summary>
    /// 敵のパラメーター(移動スピードなど)を決める
    /// </summary>
    /// <param name="enemyABility"></param>
    private void SetParameter(Ability enemyABility)
    {
        EnemyData.EnemyParameter enemyData = _enemyData.GetEnemyPar(enemyABility);
        if(enemyData != null)
        {
            _moveSpeed = enemyData.EnemyMoveSpeed;
            _enemyHP = enemyData.EnemyHP;
            _tagName = enemyData.TagName;
        }
        else
        {
            Debug.LogWarning($"Ability {enemyData}が見つかりません");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(_tagName)) //弾がTagNameであれば
        {
            Destroy(this.gameObject);
            Destroy(collision.gameObject);
            _stageManager.NowEnemyDefeatNum++;
            _stageManager.NowEnemyList.Remove(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))//Playerに触れたら
        {
            Destroy(this.gameObject);
            _playerController.PlayerHP--;
            if(_playerController.PlayerHP <= 0)
            {
                _stageManager.GameOver();
            }
            _stageManager.NowEnemyList.Remove(this.gameObject);
        }
        if (collision.gameObject.CompareTag("BigFire") || collision.gameObject.CompareTag("BigIce") || collision.gameObject.CompareTag("BigThunder"))
        {
            Destroy(this.gameObject);
            Destroy(collision.gameObject);
            _stageManager.NowEnemyDefeatNum++;
            _stageManager.NowEnemyList.Remove(this.gameObject);
        }

        else if (!collision.gameObject.CompareTag(_tagName) && !collision.gameObject.CompareTag("Floor")&&!collision.gameObject.CompareTag("Player")&&!collision.gameObject.CompareTag("Boss"))  //それ以外であれば
        {
            Destroy(collision.gameObject);
            _enemyHP--;
            if (_enemyHP <= 0)
            {
                Destroy(this.gameObject);
                _stageManager.NowEnemyDefeatNum++;
                _stageManager.NowEnemyList.Remove(this.gameObject);
            }
        }
        if(_stageManager.NowEnemyDefeatNum >= _stageManager.EnemyNextPhaseNum)
        {
            _stageManager.NowEnemyDefeatNum = 0;
            _stageManager.Phase++;
            if(_stageManager.Phase <= 2)
            {
                _stageManager.IncreasedEnemyUI();
            }
        }
    }
}
