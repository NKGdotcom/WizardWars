using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーの能力選択画面
/// </summary>
public class PlayerAbilitySelection : MonoBehaviour
{
    [Header("プレイヤーのアビリティ選択")]
    [SerializeField] private PlayerAbility playerAbility;
    private PlayerAbillityShotNormal playerAbillityShotNormal; //能力を放つ

    [Header("アビリティの画像を入れる場所")]
    [SerializeField] private Image firstAbilityImage;
    [SerializeField] private Image secondAbilityImage;

    private GameObject firstAbilityPrefab;
    private GameObject secondAbilityPrefab;

    [Header("能力一覧のページ")]
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
                    //一つ目の能力であれば
                    if (isFirstAbility) playerAbillityShotNormal.ShotFirstAbility(firstAbilityPrefab); 
                    else //一つ目の能力でなければ
                    {
                        ChangeAbility();
                    }
                }
                else if(Input.GetMouseButtonDown(1))
                {
                    //二つ目の能力であれば
                    if (!isFirstAbility) playerAbillityShotNormal.ShotSecondAbility(secondAbilityPrefab);
                    else //二つ目の能力でなければ
                    {
                        ChangeAbility();
                    }
                }
            }
        }
    }

    /// <summary>
    /// UIの選択（左右キー）処理を共通化
    /// </summary>
    private void HandleUISelectionInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)) // 右矢印押したら
        {
            abilityUIListPage[nowAbilityUIListPage].SetActive(false);
            if (nowAbilityUIListPage == abilityUIListIndexMax) nowAbilityUIListPage = 0;
            else nowAbilityUIListPage++;
            abilityUIListPage[nowAbilityUIListPage].SetActive(true); //右ページ移動完了
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) //左矢印押したら
        {
            abilityUIListPage[nowAbilityUIListPage].SetActive(false);
            if (nowAbilityUIListPage == 0) nowAbilityUIListPage = abilityUIListIndexMax;
            else nowAbilityUIListPage--; 
            abilityUIListPage[nowAbilityUIListPage].SetActive(true); //左ページ移動完了
        }
    }

    /// <summary>
    /// 一つ目の能力設定
    /// </summary>
    private void SetFirstAbility()
    {
        HandleUISelectionInput(); // UI選択ロジックを共通化
        if (Input.GetMouseButtonDown(0)) //一つ目の能力決定
        {
            firstSetAbilityIndex = nowAbilityUIListPage;
            EnumPlayerAbilityType.PlayerAbilityType selectedType = (EnumPlayerAbilityType.PlayerAbilityType)nowAbilityUIListPage;

            SetPlayerAbility(GameStateMachine.GameState.SetFirstAbility,EnumPlayerAbilityType.Instance.playerFirstAbilityType); // 1番目の能力としてセット

            abilityUIListPage[nowAbilityUIListPage].SetActive(false);
            if (nowAbilityUIListPage == abilityUIListIndexMax) nowAbilityUIListPage = 0;
            else nowAbilityUIListPage++;
            abilityUIListPage[nowAbilityUIListPage].SetActive(true);

            firstAbilityText.SetActive(false);  //一つ目の能力設定完了
            secondAbilityText.SetActive(true);  //二つ目の能力設定へ
        }
    }
    /// <summary>
    /// 二つ目の能力設定画面
    /// </summary>
    private void SetSecondAbility()
    {
        HandleUISelectionInput();

        if (nowAbilityUIListPage == firstSetAbilityIndex) //選択した能力ページを飛ばす
        {
            if (Input.GetKeyDown(KeyCode.RightArrow)) //右矢印押したら
            {
                abilityUIListPage[nowAbilityUIListPage].SetActive(false);
                if (nowAbilityUIListPage == abilityUIListIndexMax) nowAbilityUIListPage = 0;
                else nowAbilityUIListPage++;

                if (nowAbilityUIListPage == firstSetAbilityIndex)
                {
                    if (nowAbilityUIListPage == abilityUIListIndexMax) nowAbilityUIListPage = 0;
                    else nowAbilityUIListPage++;
                }
                abilityUIListPage[nowAbilityUIListPage].SetActive(true); //右ページ移動完了
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow)) // 左矢印押したら
            {
                abilityUIListPage[nowAbilityUIListPage].SetActive(false);
                if (nowAbilityUIListPage == 0) nowAbilityUIListPage = abilityUIListIndexMax;
                else nowAbilityUIListPage--;

                if (nowAbilityUIListPage == firstSetAbilityIndex)
                {
                    if (nowAbilityUIListPage == 0) nowAbilityUIListPage = abilityUIListIndexMax;
                    else nowAbilityUIListPage--;
                }
                abilityUIListPage[nowAbilityUIListPage].SetActive(true); //左ページ移動完了
            }
            return;
        }

        if (Input.GetMouseButtonDown(0)) //能力決定
        {
            SetPlayerAbility(GameStateMachine.GameState.SetSecondAbility, EnumPlayerAbilityType.Instance.playerSecondAbilityType); // 2番目の能力としてセット

            abilityUIListPage[nowAbilityUIListPage].SetActive(false);
            secondAbilityText.SetActive(false); //二つ目の能力決定

            GameStateMachine.Instance.SetState(GameStateMachine.GameState.ReadyGoTime); //ゲーム画面へ
        }
    }

    /// <summary>
    /// 選択された能力を設定
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
                    Debug.Log("能力設定完了!");
                    break;
            }
        }
    }
    /// <summary>
    /// 能力を変更
    /// </summary>
    private void ChangeAbility()
    {
        if(isFirstAbility)//2つ目の能力に変える
        {
            isFirstAbility = false;
            firstAbilityImage.gameObject.SetActive(false);
            secondAbilityImage.gameObject.SetActive(true);
        }
        else //1つ目の能力に変える
        {
            isFirstAbility = true;
            firstAbilityImage.gameObject.SetActive(true);
            secondAbilityImage.gameObject.SetActive(false);
        }
    }
}
