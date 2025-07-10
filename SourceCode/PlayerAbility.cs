using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �v���C���[�̔\�͐ݒ�(�����𑝂₹�Δ\�͂����₹��)
/// </summary>
[CreateAssetMenu(fileName = "PlayerAbility", menuName = "ScriptableObjects/Player/PlayerAbility")]
public class PlayerAbility : ScriptableObject
{
    [System.Serializable]
    public class AbilityData
    {
        [Header("�\�͈ꗗ")]
        public EnumPlayerAbilityType.PlayerAbilityType abilityType;
        [Header("�\�͂̃C���[�W�摜")]
        public Sprite abilityImage;
        [Header("���˂���e")]
        public GameObject bulletPrefab;
        [Header("���ˌ��ʉ�")]
        public AudioClip abilitySE;

        public AbilityData(EnumPlayerAbilityType.PlayerAbilityType abilityType, Sprite abilityImage, GameObject bulletPrefab, AudioClip abilitySE) // abilitySe ���� abilitySE �ɏC��
        {
            this.abilityType = abilityType;
            this.abilityImage = abilityImage;
            this.bulletPrefab = bulletPrefab;
            this.abilitySE = abilitySE;
        }
    }

    public List<AbilityData> abilityList = new List<AbilityData>();

    public AbilityData GetAbility(int _index)
    {
        if(_index >= 0 && _index < abilityList.Count)
        {
            return abilityList[_index];
        }
        return null;
    }
}