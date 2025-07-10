using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーの能力設定(ここを増やせば能力も増やせる)
/// </summary>
[CreateAssetMenu(fileName = "PlayerAbility", menuName = "ScriptableObjects/Player/PlayerAbility")]
public class PlayerAbility : ScriptableObject
{
    [System.Serializable]
    public class AbilityData
    {
        [Header("能力一覧")]
        public EnumPlayerAbilityType.PlayerAbilityType abilityType;
        [Header("能力のイメージ画像")]
        public Sprite abilityImage;
        [Header("発射する弾")]
        public GameObject bulletPrefab;
        [Header("発射効果音")]
        public AudioClip abilitySE;

        public AbilityData(EnumPlayerAbilityType.PlayerAbilityType abilityType, Sprite abilityImage, GameObject bulletPrefab, AudioClip abilitySE) // abilitySe から abilitySE に修正
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