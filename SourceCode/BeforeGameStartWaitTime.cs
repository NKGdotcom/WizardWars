using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
/// <summary>
/// �Q�[�����n�܂�O�̂�[���X�^�[�g
/// </summary>

public class BeforeGameStartWaitTime : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI readyGoText; 
    [SerializeField] private float readyWaitTime; //Ready�̑҂�����
    [SerializeField] private float goWaitTime; //Go!�̑҂�����

    void Start()
    {
        readyGoText.gameObject.SetActive(true);
        readyGoText.text = string.Empty;
    }
    // Update is called once per frame
    void Update()
    {
        if (GameStateMachine.Instance.IsReadyGoTime()) ReadyGoTextDisplay(); //�X�^�[�g�O�̑҂�����
    }
    /// <summary>
    /// ReadyGo��\������
    /// </summary>
    private void ReadyGoTextDisplay()
    {
        if (readyWaitTime > 0) //Ready...��\��
        {
            readyWaitTime -= Time.deltaTime;
            readyGoText.text = "Ready...";
        }
        else
        {
            if (goWaitTime > 0)
            {
                goWaitTime -= Time.deltaTime;
                readyGoText.text = "Go!!!";
            }
            else
            {
                readyGoText.gameObject.SetActive(false);
                GameStateMachine.Instance.SetState(GameStateMachine.GameState.Playing);
            }
        }
    }
}
