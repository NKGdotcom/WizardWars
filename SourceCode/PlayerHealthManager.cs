using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーのHP処理
/// </summary>
public class PlayerHealthManager : HealthManager
{
    public static event Action OnGameOver;

    [SerializeField] private PlayerData playerData; 
    [SerializeField] private Slider playerHPSlider;
    public override void Awake()
    {
        maxHP = playerData.PlayerHP;
        base.Awake();

        if (playerHPSlider != null)
        {
            playerHPSlider.maxValue = maxHP;
            playerHPSlider.value = currentHP;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void TakeDamage(int _damegeAmount)
    {
        base.TakeDamage(_damegeAmount);

        if (playerHPSlider != null)
        {
            playerHPSlider.value = currentHP;
        }
    }

    /// <summary>
    /// プレイヤーの死亡
    /// </summary>
    protected override void Die()
    {
        OnGameOver?.Invoke();
    }
}
