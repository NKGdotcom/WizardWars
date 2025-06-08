using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerAbility;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    [System.Serializable]
    public class EnemyParameter
    {
        public Ability EnemyAbilityType { get => _enemyAbilityType; }
        public int EnemyHP { get => _enemyHP; }
        public float EnemyMoveSpeed { get => _enemyMoveSpeed; }
        public string TagName { get => _tagName; }

        [Header("エネミーのAbilityごとのパラメーター")]
        [SerializeField] private Ability _enemyAbilityType;
        [SerializeField] private int _enemyHP;
        [SerializeField] private float _enemyMoveSpeed;
        [SerializeField] private string _tagName;

        public EnemyParameter(Ability _enemyAbilityType, int _enemyHP, float _enemyMoveSpeed, string tagName)
        {
            this._enemyAbilityType = _enemyAbilityType;
            this._enemyHP = _enemyHP;
            this._enemyMoveSpeed = _enemyMoveSpeed;
            _tagName = tagName;
        }
    }
    public List<EnemyParameter> enemyList = new List<EnemyParameter>();
    public EnemyParameter GetEnemyPar(Ability _enemyAbilityType)
    {
        foreach (EnemyParameter enemyParameter in enemyList)
        {
            if (enemyParameter.EnemyAbilityType == _enemyAbilityType)
            {
                return new EnemyParameter(enemyParameter.EnemyAbilityType,enemyParameter.EnemyHP,enemyParameter.EnemyMoveSpeed,enemyParameter.TagName);
            }
        }
        return null;
    }
}
