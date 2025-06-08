using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundManager", menuName = "ScriptableObjects/SoundManager")]
public class SoundManager : ScriptableObject
{
    [System.Serializable]
    public class SoundList
    {
        [Header("�T�E���h�̎��(BGM�܂���SE")]
        public string soundName;
        [Header("�Ή����鉹")]
        public AudioClip audioClip;

        public SoundList(string musicName, AudioClip sound)
        {
            this.soundName = musicName;
            this.audioClip = sound;
        }
    }
    public List<SoundList> soundLists = new List<SoundList>();
}
