using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ���݂̃Q�[�����(�҂����Ԃ�1�ڂ̔\�͂��Z�b�g����Ȃ�)
/// </summary>
public class GameStateMachine : MonoBehaviour
{
    public static GameStateMachine Instance { get; private set;}

    public enum GameState { SetFirstAbility,SetSecondAbility,ReadyGoTime,Playing,Paused,Result}
    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        SetState(GameState.SetFirstAbility);
    }
    /// <summary>
    /// State��ύX
    /// </summary>
    /// <param name="_newState"></param>
    public void SetState(GameState _newState)
    {
        if(CurrentState == _newState) return;

        CurrentState = _newState;
    }
    /// <summary>
    /// ��ڂ̃A�r���e�B��ݒ蒆��
    /// </summary>
    /// <returns></returns>
    public bool IsSetFirstAbility()
    {
        return CurrentState == GameState.SetFirstAbility;
    }
    /// <summary>
    /// ��ڂ̃A�r���e�B��ݒ蒆��
    /// </summary>
    /// <returns></returns>
    public bool IsSetSecondAbility()
    {
        return CurrentState == GameState.SetSecondAbility;
    }
    /// <summary>
    /// �҂����Ԓ���
    /// </summary>
    /// <returns></returns>
    public bool IsReadyGoTime()
    {
        return CurrentState == GameState.ReadyGoTime;
    }
    /// <summary>
    /// �Q�[���v���C����
    /// </summary>
    /// <returns></returns>
    public bool IsPlaying()
    {
        return CurrentState == GameState.Playing;
    }
    /// <summary>
    /// �|�[�Y��ʒ���
    /// </summary>
    /// <returns></returns>
    public bool IsPaused()
    {
        return CurrentState == GameState.Paused;
    }
    /// <summary>
    /// ���U���g��
    /// </summary>
    /// <returns></returns>
    public bool IsResult()
    {
        return CurrentState == GameState.Result;
    }
}
