using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TextHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    Vector3 textScale;
    public float hoverScale = 1.2f;
    Text hoverText;

    [SerializeField]
    int num;

    bool isSFX = false;

    void Start()
    {
        hoverText = GetComponent<Text>();
        textScale = transform.localScale;
        hoverText.color = Color.gray;

        // 옵션 텍스트
        if (gameObject.CompareTag("OptionTitle"))
        {
            for (int i = 0; i < UiManager.Instance.OptionTexts.Count; i++)
            {
                if (UiManager.Instance.OptionTexts[i].text.Equals(hoverText.text))
                {
                    num = i;
                }
            }
        }
        if (gameObject.CompareTag("GameTitle"))
        // 타이틀 텍스트
        {
            for (int i = 0; i < UiManager.Instance.GameTexts.Length; i++)
            {
                if (UiManager.Instance.GameTexts[i].name.Equals(hoverText.name))
                {
                    num = i;
                }
            }
        }
        // 게임 옵션 텍스트
        if (gameObject.CompareTag("Pause"))
        {
            for (int i = 0; i < UiManager.Instance.PlayTexts.Count; i++)
            {
                if (UiManager.Instance.PlayTexts[i].text.Equals(hoverText.text))
                {
                    num = i;
                }
            }
        }
    }

    void Update()
    {
        // 선택
        if(!UiManager.Instance.isPlay)
        {
            if (!UiManager.Instance.isOption)
            {
                if (num == UiManager.Instance.GameSelect) UiManager.Instance.isGameSelect = true;
                else UiManager.Instance.isGameSelect = false;
            }
            else
            {
                if (num == UiManager.Instance.OptionSelect) UiManager.Instance.isOptionSelect = true;
                else UiManager.Instance.isOptionSelect = false;
            }

            // 선택된 것 크기 키우고 색 바꾸기
            if (UiManager.Instance.isGameSelect || UiManager.Instance.isOptionSelect)
            {
                transform.localScale = textScale * hoverScale;
                hoverText.color = Color.white;

                // 효과음
                if (!isSFX)
                {
                    AudioManager.Instance.PlaySound2D("CoinDebt");
                    isSFX = true;
                }


                // 선택되어 있는게 있을 때 스페이스바나 엔터 누르면 실행
                if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
                {
                    UiManager.Instance.SelectOption(hoverText, num);
                }
            }
            else
            {
                transform.localScale = textScale;
                hoverText.color = Color.gray;
            }
        }
        else
        {
            if (num == UiManager.Instance.PlaySelect) UiManager.Instance.isPlaySelect = true;
            else UiManager.Instance.isPlaySelect = false;

            // 선택된 것 크기 키우고 색 바꾸기
            if (UiManager.Instance.isPlaySelect)
            {
                transform.localScale = textScale * hoverScale;
                hoverText.color = Color.white;

                // 선택되어 있는게 있을 때 스페이스바나 엔터 누르면 실행
                if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
                {
                    UiManager.Instance.SelectOption(hoverText, num);
                }
            }
            else
            {
                transform.localScale = textScale;
                hoverText.color = Color.gray;
            }
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!UiManager.Instance.isPlay)
        {
            // 전에 선택된 것의 텍스트 계속 선택되게 하기
            if (!UiManager.Instance.isOption)
            {
                for (int i = 0; i < UiManager.Instance.GameTexts.Length; i++)
                {
                    if (UiManager.Instance.GameTexts[i].text.Equals(hoverText.text))
                    {
                        UiManager.Instance.GameSelect = i;
                        isSFX = false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < UiManager.Instance.OptionTexts.Count; i++)
                {
                    if (UiManager.Instance.OptionTexts[i].text.Equals(hoverText.text))
                    {
                        UiManager.Instance.OptionSelect = i;
                        isSFX = false;
                    }
                }
            }
        }
        else
        // 게임 플레이 옵션
        {
            Debug.Log(num);
            for (int i = 0; i < UiManager.Instance.PlayTexts.Count; i++)
            {
                if (UiManager.Instance.PlayTexts[i].text.Equals(hoverText.text))
                {
                    UiManager.Instance.PlaySelect = i;
                    isSFX = false;
                }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (UiManager.Instance.isGameSelect) UiManager.Instance.isGameSelect = false;
        if (UiManager.Instance.isOptionSelect) UiManager.Instance.isOptionSelect = false;
        if (UiManager.Instance.isPlaySelect) UiManager.Instance.isPlaySelect = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UiManager.Instance.SelectOption(hoverText, num);
    }
}
