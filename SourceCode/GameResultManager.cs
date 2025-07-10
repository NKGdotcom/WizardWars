using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームリザルトを設定
/// </summary>
public class GameResultManager : MonoBehaviour
{
    [Header("ゲームクリアとゲームオーバーUI")]
    [SerializeField] private GameObject gameClearUI;
    [SerializeField] private GameObject gameOverUI;
    private void OnEnable()
    {
        PlayerHealthManager.OnGameOver += GameOver;
    }
    private void OnDisable()
    {
        PlayerHealthManager.OnGameOver -= GameOver;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void GameOver()
    {

    }
}
