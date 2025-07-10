using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HP�̏������܂Ƃ߂�
/// </summary>
public abstract class HealthManager : MonoBehaviour
{
    [Header("�ő�HP")]
    [SerializeField] protected int maxHP;
    protected int currentHP; //����HP

    public event Action<int, int> OnHealthChange; //HP�̒l���ύX

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
    /// �_���[�W���󂯂��
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
    /// ���S
    /// </summary>
    protected abstract void Die();
}
