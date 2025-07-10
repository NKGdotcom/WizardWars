using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HPの処理をまとめる
/// </summary>
public abstract class HealthManager : MonoBehaviour
{
    [Header("最大HP")]
    [SerializeField] protected int maxHP;
    protected int currentHP; //現在HP

    public event Action<int, int> OnHealthChange; //HPの値が変更

    public virtual void Awake()
    {
        currentHP = maxHP;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// ダメージを受ける量
    /// </summary>
    /// <param name="_damegeAmount"></param>
    public virtual void TakeDamage(int _damegeAmount)
    {
        currentHP -= _damegeAmount;
        if(currentHP < 0)
        {
            currentHP = 0;
        }

        if(currentHP <= 0)
        {
            Die();
        }
    }
    /// <summary>
    /// 死亡
    /// </summary>
    protected abstract void Die();
}
