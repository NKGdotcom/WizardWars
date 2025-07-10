using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
/// <summary>
/// ゲームが始まる前のよーいスタート
/// </summary>

public class BeforeGameStartWaitTime : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI readyGoText; 
    [SerializeField] private float readyWaitTime; //Readyの待ち時間
    [SerializeField] private float goWaitTime; //Go!の待ち時間

    void Start()
    {
        readyGoText.gameObject.SetActive(true);
        readyGoText.text = string.Empty;
    }
    // Update is called once per frame
    void Update()
    {
        if (GameStateMachine.Instance.IsReadyGoTime()) ReadyGoTextDisplay(); //スタート前の待ち時間
    }
    /// <summary>
    /// ReadyGoを表示する
    /// </summary>
    private void ReadyGoTextDisplay()
    {
        if (readyWaitTime > 0) //Ready...を表示
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
