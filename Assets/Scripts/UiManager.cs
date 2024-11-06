using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static GameData;
using static UnityEditor.Experimental.GraphView.GraphView;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

    [Header("# Option Info #")]
    public Image[] TitleImages;
    public Text[] TitleTexts;

    public GameObject GameOption;
    public Text[] GameTexts;
    public int GameSelect;

    public bool isOption = false;
    bool isAnyButton = false;
    public bool isGameSelect = false;

    public GameObject Option;
    public List<Text> OptionTexts;
    public int OptionSelect;
    public bool isOptionSelect = false;

    public GameObject PlayOtion;
    public List<Text> PlayTexts;
    public int PlaySelect;
    public bool isPlaySelect = false;

    [Header("# Play Info #")]
    public bool isPlay = false;
    bool isOneEnter = false;
    bool isTwoEnter = false;
    public int playerNum;
    public int playerSelectNum;
    public GameObject[] gameEnter = new GameObject[2];
    public GameObject gameUi;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        DOTween.Init(false, true, LogBehaviour.Verbose).SetCapacity(200, 50);

        GameTexts = GameOption.GetComponentsInChildren<Text>(true);
        OptionTexts = new List<Text>();
        OptionTexts.AddRange(Option.GetComponentsInChildren<Text>(true));
        int j = OptionTexts.Count;
        for (int i = 0; i < j; i++)
        {
            if (!OptionTexts[i].CompareTag("OptionTitle"))
            {
                OptionTexts.Remove(OptionTexts[i]);
                i--; j--;
            }
        }

        PlayTexts = new List<Text>();
        PlayTexts.AddRange(PlayOtion.GetComponentsInChildren<Text>(true));
        PlayTexts.Remove(PlayTexts[0]);
    }


    void Update()
    {
        if (!isPlay)
        {
            // 아무 버튼 클릭
            if (!isAnyButton)
            {
                if (Input.anyKeyDown)
                {
                    TitleMove(true);
                }
            }
            else
            // 여기는 아무 버튼 누르고 난 후 선택하는 곳
            {
                // ESC 누르면 다시 아무 버튼 창으로
                if (isAnyButton)
                {
                    if (Input.GetKeyDown(KeyCode.Escape) && !isOption)
                    {
                        TitleMove(false);
                    }
                    // 위로 이동 (W 키)
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        if (GameSelect <= 0) return;
                        GameSelect--;
                    }
                    // 아래로 이동 (S 키)
                    else if (Input.GetKeyDown(KeyCode.S))
                    {
                        if (GameSelect >= GameTexts.Length - 1) return;
                        GameSelect++;
                    }
                }

                if (isOption)
                {
                    // 옵션창 있을 때 ESC 누르면 선택창으로 돌아가기
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        ShowOptionWindow(false);
                        ShowTitle(false);
                    }

                    // 위로 이동 (W 키)
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        if (OptionSelect <= 0) return;
                        OptionSelect--;
                        AudioManager.Instance.PlaySound2D("CoinDebt");
                    }
                    // 아래로 이동 (S 키)
                    else if (Input.GetKeyDown(KeyCode.S))
                    {
                        if (OptionSelect >= OptionTexts.Count - 1) return;
                        OptionSelect++;
                        AudioManager.Instance.PlaySound2D("CoinDebt");
                    }
                }
            }
        }
        else
        {
            // 플레이어 선택에서 esc 누르면 타이틀로
            if (Input.GetKeyDown(KeyCode.Escape) && isOneEnter)
            {
                // 다 초기화 후 타이틀로
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && !isOption)
            // 게임 esc창
            {

            }
            else
            // 옵션창 나가기
            {

            }

            // 플레이어 선택에서 space 누르면 캐릭터 나오기
            if (Input.GetKeyDown(KeyCode.Space) && isOneEnter)
            {
                switch (playerNum)
                {
                    case 1:
                        gameEnter[playerNum - 1].gameObject.SetActive(false);
                        isOneEnter = false;
                        break;
                    case 2:
                        // 플레이어 선택
                        switch(playerSelectNum)
                        {
                            case 0:
                                gameEnter[playerNum - 1].gameObject.SetActive(false);
                                break;
                            case 1:
                                gameEnter[playerNum].gameObject.SetActive(false);
                                isTwoEnter = false;
                                break;
                        }
                        break;
                }
                gameUi.SetActive(true);
            }

            //// 위로 이동 (W 키)
            //if (Input.GetKeyDown(KeyCode.W))
            //{
            //    if (PlaySelect <= 0) return;
            //    PlaySelect--;
            //    AudioManager.Instance.PlaySound2D("CoinDebt");
            //}
            //// 아래로 이동 (S 키)
            //else if (Input.GetKeyDown(KeyCode.S))
            //{
            //    if (PlaySelect >= PlayTexts.Count - 1) return;
            //    PlaySelect++;
            //    AudioManager.Instance.PlaySound2D("CoinDebt");
            //}
        }
    }

    // 아무 버튼 눌렀을 때 타이틀 이동
    public void TitleMove(bool _isBool)
    {
        if(_isBool)
        {
            TitleTexts[0].gameObject.SetActive(!_isBool);
            TitleImages[0].DOColor(Color.grey, 0.3f);
            TitleImages[1].rectTransform.DOAnchorPosY(220, 0.3f).OnComplete(() =>
            {
                GameOption.SetActive(_isBool);
                isAnyButton = _isBool;
            });
        }
        else
        {
            isAnyButton = _isBool;
            TitleImages[0].DOColor(Color.white, 0.3f);
            TitleImages[1].rectTransform.DOAnchorPosY(0, 0.3f);
            TitleTexts[0].gameObject.SetActive(!_isBool);
            GameOption.SetActive(_isBool);
        }
    }

    public void ShowOptionWindow(bool _isShow)
    {
        // 예외처리
        if (!isAnyButton) return;

        Option.gameObject.SetActive(_isShow);
        isOption = _isShow;
        if(!isPlay) GameOption.SetActive(!_isShow);
    }

    // 옵션창 들어가면 타이틀 점점 사라지게
    public void ShowTitle(bool _isShow)
    {
        if(_isShow)
        {
            TitleImages[1].DOFade(0, 0.75f);
        }
        else
        {
            TitleImages[1].DOFade(1, 1.5f);
        }
    }

    // 게임에서 옵션 선택 시 게임 옵션 사라고 옵션 나옴. 바탕은 블러처리
    public void ShowGameOption(bool _isBool)
    {
        ShowOptionWindow(_isBool);
        isAnyButton = _isBool;
        isPlay = _isBool;
    }

    public void GamePlay(bool _isBool, int _player)
    {
        Debug.Log(_isBool);
        Debug.Log(_player);

        ShowOptionWindow(!_isBool);
        TitleMove(!_isBool);
        for (int i = 0; i < TitleImages.Length; i++)
        {
            TitleImages[i].gameObject.SetActive(!_isBool);
        }
        TitleTexts[0].gameObject.SetActive(!_isBool);

        isAnyButton = !_isBool;

        // 게임 시작 시 필요한 것들
        switch (_player)
        {
            case 1:
                isOneEnter = _isBool;
                gameEnter[_player - 1].gameObject.SetActive(_isBool);
                break;
            case 2:
                isTwoEnter = _isBool;
                gameEnter[_player].gameObject.SetActive(_isBool);
                gameEnter[_player - 1].gameObject.SetActive(_isBool);
                break;
        }
        
        isPlay = _isBool;
    }

    public void SelectOption(Text _text, int _select)
    {
        if(isPlay)
        {
            switch (_select)
            {
                case (int)GameOptionType.Continue:
                    Debug.Log(_text.text);
                    break;
                case (int)GameOptionType.GoTitle:
                    GamePlay(false, playerNum);
                    Debug.Log(_text.text);
                    break;
                case (int)GameOptionType.Option:
                    Debug.Log(_text.text);
                    break;
                case (int)GameOptionType.Exit:
                    Application.Quit();
                    break;
                default:
                    Debug.Log(_text.text);
                    break;
            }
        }
        else
        {
            // 게임 선택
            if (!isOption)
            {
                switch (_select)
                {
                    case (int)GameTextType.Single:
                        playerNum = 1;
                        GamePlay(true, playerNum);
                        break;
                    case (int)GameTextType.Two:
                        playerNum = 2;
                        playerSelectNum = 0;
                        GamePlay(true, playerNum);
                        Debug.Log(_text.text);
                        break;
                    case (int)GameTextType.Versus:
                        Debug.Log(_text.text);
                        break;
                    case (int)GameTextType.Option:
                        ShowOptionWindow(true);
                        ShowTitle(true);
                        Debug.Log(_text.text);
                        break;
                    case (int)GameTextType.Credits:
                        Debug.Log(_text.text);
                        break;
                    case (int)GameTextType.Quit:
                        Debug.Log(_text.text);
                        Application.Quit();
                        break;
                }
            }
            // 옵션 선택
            else
            {
                switch (_select)
                {
                    case (int)OptionTextType.KeyConfig:
                        Debug.Log(_text.text);
                        break;
                    case (int)OptionTextType.SaveReset:
                        Debug.Log(_text.text);
                        break;
                    case (int)OptionTextType.Default:
                        Debug.Log(_text.text);
                        break;
                    case (int)OptionTextType.Back:
                        if(!isPlay) ShowOptionWindow(false);
                        {

                        }
                        Debug.Log(_text.text);
                        break;
                    default:
                        Debug.Log(_text.text);
                        break;
                }
            }
        }
    }
}
