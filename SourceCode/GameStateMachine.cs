using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 現在のゲーム状態(待ち時間や1つ目の能力をセットするなど)
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
    /// Stateを変更
    /// </summary>
    /// <param name="_newState"></param>
    public void SetState(GameState _newState)
    {
        if(CurrentState == _newState) return;

        CurrentState = _newState;
    }
    /// <summary>
    /// 一つ目のアビリティを設定中か
    /// </summary>
    /// <returns></returns>
    public bool IsSetFirstAbility()
    {
        return CurrentState == GameState.SetFirstAbility;
    }
    /// <summary>
    /// 二つ目のアビリティを設定中か
    /// </summary>
    /// <returns></returns>
    public bool IsSetSecondAbility()
    {
        return CurrentState == GameState.SetSecondAbility;
    }
    /// <summary>
    /// 待ち時間中か
    /// </summary>
    /// <returns></returns>
    public bool IsReadyGoTime()
    {
        return CurrentState == GameState.ReadyGoTime;
    }
    /// <summary>
    /// ゲームプレイ中か
    /// </summary>
    /// <returns></returns>
    public bool IsPlaying()
    {
        return CurrentState == GameState.Playing;
    }
    /// <summary>
    /// ポーズ画面中か
    /// </summary>
    /// <returns></returns>
    public bool IsPaused()
    {
        return CurrentState == GameState.Paused;
    }
    /// <summary>
    /// リザルトか
    /// </summary>
    /// <returns></returns>
    public bool IsResult()
    {
        return CurrentState == GameState.Result;
    }
}
