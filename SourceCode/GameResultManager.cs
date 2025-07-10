using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Q�[�����U���g��ݒ�
/// </summary>
public class GameResultManager : MonoBehaviour
{
    [Header("�Q�[���N���A�ƃQ�[���I�[�o�[UI")]
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
