using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TextBlink : MonoBehaviour
{
    Text text;

    public void Awake()
    {
        text = GetComponent<Text>();
    }

    public void Update()
    {
        if (!gameObject.activeSelf) StopAllCoroutines();
    }

    private void OnEnable()
    {
        StartCoroutine(FTextOut());
    }

    public IEnumerator FTextIn()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        while (text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / 2.0f));
            yield return null;
        }
        StartCoroutine(FTextOut());
    }

    public IEnumerator FTextOut()
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        while (text.color.a > 0.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / 2.0f));
            yield return null;
        }
        StartCoroutine(FTextIn());
    }

}
