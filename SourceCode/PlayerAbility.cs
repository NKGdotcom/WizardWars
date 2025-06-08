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
        [Header("能力一覧")]
        public Ability abilityType;
        [Header("能力のイメージ画像")]
        public Sprite abilityImage;
        [Header("発射する弾")]
        public GameObject bulletPurefabs;
        [Header("発射効果音")]
        public AudioClip abilitySE;
        [Header("必殺技名")]
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
