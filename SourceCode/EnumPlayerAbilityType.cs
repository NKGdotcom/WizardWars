using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameStateMachine;

/// <summary>
/// �v���C���[�̔\�͂�ݒ肷��
/// </summary>
public class EnumPlayerAbilityType : MonoBehaviour
{
    public static EnumPlayerAbilityType Instance { get; private set; }
    public enum PlayerAbilityType { None,FireAbilityType,IceAbilityType,ThunderAbilityType }
    public PlayerAbilityType playerFirstAbilityType { get; private set;}
    public PlayerAbilityType playerSecondAbilityType { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        playerFirstAbilityType = PlayerAbilityType.None;
        playerSecondAbilityType = PlayerAbilityType.None;
    }
    /// <summary>
    /// Ability��ύX
    /// </summary>
    /// <param name="_newState"></param>
    public void SetAbility(PlayerAbilityType _nowAbility, PlayerAbilityType _newAbility)
    {
        if (_nowAbility == _newAbility) return;

        _newAbility = _nowAbility;
    }
}
