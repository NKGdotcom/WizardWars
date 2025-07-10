using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.WebCam;

/// <summary>
/// �v���C���[��MP����
/// </summary>
public class PlayerMagicPointManager : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Slider playerMPSlider;
    private float playerMP;
    private float playerMaxMP;

    [Header("MP���񕜂���Ƃ��҂���")]
    [SerializeField] private float playerRecoverWaitTime;
    private float playerRecoverDelay;
    private bool recoverTime = false;
    //1�b�Ԃ�1��
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
    /// MP�̉�
    /// </summary>
    /// <returns></returns>
    private IEnumerator RecoverMP()
    {
        if (recoverTime)//��x�����������
        {
            recoverTime = false;
            playerRecoverDelay -= Time.deltaTime;
            if (playerRecoverDelay < 0)
            {
                while(playerMP <  playerMaxMP)//1�b���Ƃ�Mp1��
                {
                    playerMP++;
                    yield return new WaitForSeconds(recoverSecond);
                }
                playerMP = playerMaxMP;
            }
        }
    }
    /// <summary>
    /// �ʏ�U����MP���X���C�_�[�Ŕ��f
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
