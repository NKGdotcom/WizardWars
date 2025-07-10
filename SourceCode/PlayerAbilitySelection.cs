using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �v���C���[�̔\�͑I�����
/// </summary>
public class PlayerAbilitySelection : MonoBehaviour
{
    [Header("�v���C���[�̃A�r���e�B�I��")]
    [SerializeField] private PlayerAbility playerAbility;
    private PlayerAbillityShotNormal playerAbillityShotNormal; //�\�͂����

    [Header("�A�r���e�B�̉摜������ꏊ")]
    [SerializeField] private Image firstAbilityImage;
    [SerializeField] private Image secondAbilityImage;

    private GameObject firstAbilityPrefab;
    private GameObject secondAbilityPrefab;

    [Header("�\�͈ꗗ�̃y�[�W")]
    [SerializeField] private GameObject[] abilityUIListPage;
    private int nowAbilityUIListPage = 0;
    private int abilityUIListIndexMax;
    private int firstSetAbilityIndex;

    [SerializeField] private GameObject firstAbilityText;
    [SerializeField] private GameObject secondAbilityText;

    private bool isFirstAbility = true;

    // Start is called before the first frame update
    void Start()
    {
        abilityUIListPage[0].SetActive(true);
        firstAbilityText.SetActive(true);
        secondAbilityText.SetActive(false);
        abilityUIListIndexMax = abilityUIListPage.Length - 1;

        playerAbillityShotNormal = GetComponent<PlayerAbillityShotNormal>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateMachine.Instance != null)
        {
            if (GameStateMachine.Instance.IsSetFirstAbility())
            {
                SetFirstAbility();
            }
            else if (GameStateMachine.Instance.IsSetSecondAbility())
            {
                SetSecondAbility();
            }
            else if(GameStateMachine.Instance.IsPlaying())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    //��ڂ̔\�͂ł����
                    if (isFirstAbility) playerAbillityShotNormal.ShotFirstAbility(firstAbilityPrefab); 
                    else //��ڂ̔\�͂łȂ����
                    {
                        ChangeAbility();
                    }
                }
                else if(Input.GetMouseButtonDown(1))
                {
                    //��ڂ̔\�͂ł����
                    if (!isFirstAbility) playerAbillityShotNormal.ShotSecondAbility(secondAbilityPrefab);
                    else //��ڂ̔\�͂łȂ����
                    {
                        ChangeAbility();
                    }
                }
            }
        }
    }

    /// <summary>
    /// UI�̑I���i���E�L�[�j���������ʉ�
    /// </summary>
    private void HandleUISelectionInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)) // �E��󉟂�����
        {
            abilityUIListPage[nowAbilityUIListPage].SetActive(false);
            if (nowAbilityUIListPage == abilityUIListIndexMax) nowAbilityUIListPage = 0;
            else nowAbilityUIListPage++;
            abilityUIListPage[nowAbilityUIListPage].SetActive(true); //�E�y�[�W�ړ�����
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) //����󉟂�����
        {
            abilityUIListPage[nowAbilityUIListPage].SetActive(false);
            if (nowAbilityUIListPage == 0) nowAbilityUIListPage = abilityUIListIndexMax;
            else nowAbilityUIListPage--; 
            abilityUIListPage[nowAbilityUIListPage].SetActive(true); //���y�[�W�ړ�����
        }
    }

    /// <summary>
    /// ��ڂ̔\�͐ݒ�
    /// </summary>
    private void SetFirstAbility()
    {
        HandleUISelectionInput(); // UI�I�����W�b�N�����ʉ�
        if (Input.GetMouseButtonDown(0)) //��ڂ̔\�͌���
        {
            firstSetAbilityIndex = nowAbilityUIListPage;
            EnumPlayerAbilityType.PlayerAbilityType selectedType = (EnumPlayerAbilityType.PlayerAbilityType)nowAbilityUIListPage;

            SetPlayerAbility(GameStateMachine.GameState.SetFirstAbility,EnumPlayerAbilityType.Instance.playerFirstAbilityType); // 1�Ԗڂ̔\�͂Ƃ��ăZ�b�g

            abilityUIListPage[nowAbilityUIListPage].SetActive(false);
            if (nowAbilityUIListPage == abilityUIListIndexMax) nowAbilityUIListPage = 0;
            else nowAbilityUIListPage++;
            abilityUIListPage[nowAbilityUIListPage].SetActive(true);

            firstAbilityText.SetActive(false);  //��ڂ̔\�͐ݒ芮��
            secondAbilityText.SetActive(true);  //��ڂ̔\�͐ݒ��
        }
    }
    /// <summary>
    /// ��ڂ̔\�͐ݒ���
    /// </summary>
    private void SetSecondAbility()
    {
        HandleUISelectionInput();

        if (nowAbilityUIListPage == firstSetAbilityIndex) //�I�������\�̓y�[�W���΂�
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) //�E��󉟂�����
            {
                abilityUIListPage[nowAbilityUIListPage].SetActive(false);
                if (nowAbilityUIListPage == abilityUIListIndexMax) nowAbilityUIListPage = 0;
                else nowAbilityUIListPage++;

                if (nowAbilityUIListPage == firstSetAbilityIndex)
                {
                    if (nowAbilityUIListPage == abilityUIListIndexMax) nowAbilityUIListPage = 0;
                    else nowAbilityUIListPage++;
                }
                abilityUIListPage[nowAbilityUIListPage].SetActive(true); //�E�y�[�W�ړ�����
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) // ����󉟂�����
            {
                abilityUIListPage[nowAbilityUIListPage].SetActive(false);
                if (nowAbilityUIListPage == 0) nowAbilityUIListPage = abilityUIListIndexMax;
                else nowAbilityUIListPage--;

                if (nowAbilityUIListPage == firstSetAbilityIndex)
                {
                    if (nowAbilityUIListPage == 0) nowAbilityUIListPage = abilityUIListIndexMax;
                    else nowAbilityUIListPage--;
                }
                abilityUIListPage[nowAbilityUIListPage].SetActive(true); //���y�[�W�ړ�����
            }
            return;
        }

        if (Input.GetMouseButtonDown(0)) //�\�͌���
        {
            SetPlayerAbility(GameStateMachine.GameState.SetSecondAbility, EnumPlayerAbilityType.Instance.playerSecondAbilityType); // 2�Ԗڂ̔\�͂Ƃ��ăZ�b�g

            abilityUIListPage[nowAbilityUIListPage].SetActive(false);
            secondAbilityText.SetActive(false); //��ڂ̔\�͌���

            GameStateMachine.Instance.SetState(GameStateMachine.GameState.ReadyGoTime); //�Q�[����ʂ�
        }
    }

    /// <summary>
    /// �I�����ꂽ�\�͂�ݒ�
    /// </summary>
    /// <param name="_nowState"></param>
    public void SetPlayerAbility(GameStateMachine.GameState _nowState, EnumPlayerAbilityType.PlayerAbilityType _abilityType)
    {
        PlayerAbility.AbilityData selectedAbilityData = playerAbility.GetAbility(nowAbilityUIListPage); //

        if (selectedAbilityData != null)
        {
            switch (_nowState)
            {
                case (GameStateMachine.GameState.SetFirstAbility):
                    _abilityType = selectedAbilityData.abilityType;
                    firstAbilityImage.sprite = selectedAbilityData.abilityImage;
                    firstAbilityPrefab = selectedAbilityData.bulletPrefab;
                    GameStateMachine.Instance.SetState(GameStateMachine.GameState.SetSecondAbility);
                    break;
                case (GameStateMachine.GameState.SetSecondAbility):
                    _abilityType = selectedAbilityData.abilityType;
                    secondAbilityImage.sprite = selectedAbilityData.abilityImage;
                    secondAbilityPrefab = selectedAbilityData.bulletPrefab;
                    Debug.Log("�\�͐ݒ芮��!");
                    break;
            }
        }
    }
    /// <summary>
    /// �\�͂�ύX
    /// </summary>
    private void ChangeAbility()
    {
        if(isFirstAbility)//2�ڂ̔\�͂ɕς���
        {
            isFirstAbility = false;
            firstAbilityImage.gameObject.SetActive(false);
            secondAbilityImage.gameObject.SetActive(true);
        }
        else //1�ڂ̔\�͂ɕς���
        {
            isFirstAbility = true;
            firstAbilityImage.gameObject.SetActive(true);
            secondAbilityImage.gameObject.SetActive(false);
        }
    }
}
