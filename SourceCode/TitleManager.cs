using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private Fade _fade;
    [SerializeField] private Button _gameSceneButton;
    [SerializeField] private Button _optionButton;
    [SerializeField] private Button _howToPlayRightButton;
    [SerializeField] private GameObject _howToPlayRightObj;
    [SerializeField] private Button _howToPlayLeftButton;
    [SerializeField] private GameObject _howToPlayLeftObj;
    [SerializeField] private Button _backTitleButton;
    [SerializeField] private Animator _titleAnimator;

    [SerializeField] private GameObject _howToPlayUI;
    [SerializeField] private GameObject[] _howToPlayPage;
    private int _pageNum; //ページの数
    private int _nowPageNum; //現在のページ番号

    [SerializeField] private string _loadSceneName;

    private bool _title;

    private void Start()
    {
        _title = true;
        if(_gameSceneButton != null)
        {
            _gameSceneButton.onClick.AddListener(() => OnNextScene(_loadSceneName));
        }
        if(_optionButton != null)
        {
            _optionButton.onClick.AddListener(() => OpenHowToPlayPage(_nowPageNum));
        }
        if(_howToPlayRightButton != null)
        {
            _howToPlayRightButton.onClick.AddListener(() => RightPage());
        }
        if(_howToPlayLeftButton != null)
        {
            _howToPlayLeftButton.onClick.AddListener(() => LeftPage());
        }
        if(_backTitleButton != null)
        {
            _backTitleButton.onClick.AddListener(() => BackTitle());
        }

        _pageNum = _howToPlayPage.Length;
        _nowPageNum = 0;
        StartCoroutine(FadeOut());
    }
    private void Update()
    {
        if (_title)
        {
            if (Input.anyKey&&!Input.GetKeyDown(KeyCode.Print))
            {
                _title = false;
                _titleAnimator.SetTrigger("GoSelection");
            }
        }
    }
    // Start is called before the first frame update
    /// <summary>
    /// ゲームシーンに移動
    /// </summary>
    /// <param name="LoadSceneName"></param>
    public void OnNextScene(string LoadSceneName)
    {
        _fade.FadeIn(1f, () => SceneManager.LoadScene(LoadSceneName));
    }
    /// <summary>
    /// 操作方法UIを開く
    /// </summary>
    public void OpenHowToPlayPage(int pageNum)
    {
        _titleAnimator.SetBool("HowToPlay", true);
        _howToPlayUI.SetActive(true);
        _howToPlayPage[pageNum].SetActive(true);
        _howToPlayRightObj.SetActive(true);
        _howToPlayLeftObj.SetActive(true);

        if (pageNum == _pageNum-1)
        {
            _howToPlayRightObj.SetActive(false);
        }
        if(pageNum == 0)
        {
            _howToPlayLeftObj.SetActive(false);
        }
    }
    /// <summary>
    /// 右に移動
    /// </summary>
    private void RightPage()
    {
        _howToPlayPage[_nowPageNum].SetActive(false);
        _nowPageNum++;
        _howToPlayPage[_nowPageNum].SetActive(true);
        _howToPlayRightObj.SetActive(true);
        _howToPlayLeftObj.SetActive(true);
        if (_nowPageNum == _pageNum-1)
        {
            _howToPlayRightObj.SetActive(false);
        }
    }
    private void LeftPage()
    {
        _howToPlayPage[_nowPageNum].SetActive(false);
        _nowPageNum--;
        _howToPlayPage[_nowPageNum].SetActive(true);
        _howToPlayRightObj.SetActive(true);
        _howToPlayLeftObj.SetActive(true);
        if (_nowPageNum == 0)
        {
            _howToPlayLeftObj.SetActive(false);
        }
    }
    private void BackTitle()
    {
        _titleAnimator.SetBool("HowToPlay", false);
    }
    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(0.5f);
        _fade.FadeOut(1f);
        yield break;
    }
}
