using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PlayerAbility", menuName = "ScriptableObjects/Player/PlayerAbility")]
public class PlayerAbility : ScriptableObject
{
    [System.Serializable]
    public class KindOfAbility
    {
        [Header("�\�͈ꗗ")]
        public Ability abilityType;
        [Header("�\�͂̃C���[�W�摜")]
        public Sprite abilityImage;
        [Header("���˂���e")]
        public GameObject bulletPurefabs;
        [Header("���ˌ��ʉ�")]
        public AudioClip abilitySE;
        [Header("�K�E�Z��")]
        public string specialName;
        public KindOfAbility(Ability abilityType, Sprite abilityImage, GameObject bulletPrefabs,AudioClip abilitySe, string specialName)
        {
            this.abilityType = abilityType;
            this.abilityImage = abilityImage;  
            this.bulletPurefabs = bulletPrefabs;
            this.abilitySE = abilitySe;
            this.specialName = specialName;
        }
    }
    public List<KindOfAbility> abilityList = new List<KindOfAbility>();

    public KindOfAbility GetAbility(Ability abilityType)
    {
        foreach (KindOfAbility ability in abilityList)
        {
            if (ability.abilityType == abilityType)
            {
                return new KindOfAbility(ability.abilityType, ability.abilityImage, ability.bulletPurefabs,ability.abilitySE,ability.specialName);
            }
        }
        return null;
    }
}
