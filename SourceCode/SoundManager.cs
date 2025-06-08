using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundManager", menuName = "ScriptableObjects/SoundManager")]
public class SoundManager : ScriptableObject
{
    [System.Serializable]
    public class SoundList
    {
        [Header("ƒTƒEƒ“ƒh‚ÌŽí—Þ(BGM‚Ü‚½‚ÍSE")]
        public string soundName;
        [Header("‘Î‰ž‚·‚é‰¹")]
        public AudioClip audioClip;

        public SoundList(string musicName, AudioClip sound)
        {
            this.soundName = musicName;
            this.audioClip = sound;
        }
    }
    public List<SoundList> soundLists = new List<SoundList>();
}
