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

        // �ɼ� �ؽ�Ʈ
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
        // Ÿ��Ʋ �ؽ�Ʈ
        {
            for (int i = 0; i < UiManager.Instance.GameTexts.Length; i++)
            {
                if (UiManager.Instance.GameTexts[i].name.Equals(hoverText.name))
                {
                    num = i;
                }
            }
        }
        // ���� �ɼ� �ؽ�Ʈ
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
        // ����
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

            // ���õ� �� ũ�� Ű��� �� �ٲٱ�
            if (UiManager.Instance.isGameSelect || UiManager.Instance.isOptionSelect)
            {
                transform.localScale = textScale * hoverScale;
                hoverText.color = Color.white;

                // ȿ����
                if (!isSFX)
                {
                    AudioManager.Instance.PlaySound2D("CoinDebt");
                    isSFX = true;
                }


                // ���õǾ� �ִ°� ���� �� �����̽��ٳ� ���� ������ ����
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

            // ���õ� �� ũ�� Ű��� �� �ٲٱ�
            if (UiManager.Instance.isPlaySelect)
            {
                transform.localScale = textScale * hoverScale;
                hoverText.color = Color.white;

                // ���õǾ� �ִ°� ���� �� �����̽��ٳ� ���� ������ ����
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
            // ���� ���õ� ���� �ؽ�Ʈ ��� ���õǰ� �ϱ�
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
        // ���� �÷��� �ɼ�
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
