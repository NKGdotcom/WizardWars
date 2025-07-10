using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.WebCam;

/// <summary>
/// プレイヤーのMP処理
/// </summary>
public class PlayerMagicPointManager : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Slider playerMPSlider;
    private float playerMP;
    private float playerMaxMP;

    [Header("MPが回復するとき待つ時間")]
    [SerializeField] private float playerRecoverWaitTime;
    private float playerRecoverDelay;
    private bool recoverTime = false;
    //1秒間に1回復
    private const float recoverSecond = 1;

    // Start is called before the first frame update
    void Start()
    {
        playerMP = playerMaxMP = playerData.PlayerMP;
        playerRecoverDelay = playerRecoverWaitTime;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(RecoverMP());
    }
    /// <summary>
    /// MPの回復
    /// </summary>
    /// <returns></returns>
    private IEnumerator RecoverMP()
    {
        if (recoverTime)//一度球を放ったら
        {
            recoverTime = false;
            playerRecoverDelay -= Time.deltaTime;
            if (playerRecoverDelay < 0)
            {
                while(playerMP <  playerMaxMP)//1秒ごとにMp1回復
                {
                    playerMP++;
                    yield return new WaitForSeconds(recoverSecond);
                }
                playerMP = playerMaxMP;
            }
        }
    }
    /// <summary>
    /// 通常攻撃のMPをスライダーで反映
    /// </summary>
    public void ReduceMPNormalShot()
    {
        if (playerMP < 0) return;

        recoverTime = true;
        playerRecoverDelay = playerRecoverWaitTime;
        playerMP--;
        playerMPSlider.value = playerMP / playerMaxMP; 
    }
}
