using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HUD : MonoBehaviour
{
    public GameData.InfoType type;

    Slider mySlider;
    Text myText;

    private void Awake()
    {
        mySlider = GetComponent<Slider>();
        myText = GetComponent<Text>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case GameData.InfoType.Health:
                float curHP = GameManager.Instance.health;
                float maxHP = GameManager.Instance.maxHealth;
                if(gameObject.GetComponent<Slider>() != null) mySlider.value = curHP / maxHP;
               else myText.text = string.Format("{0:F0}/{1:F0}", curHP, maxHP);
                break;
            case GameData.InfoType.Magic:
                float curMP = GameManager.Instance.magic;
                float maxMP = GameManager.Instance.maxMagic;
                mySlider.value = curMP / maxMP;
                break;
            case GameData.InfoType.Coin:

                break;
            case GameData.InfoType.HearthStone:

                break;
            case GameData.InfoType.Enemy:

                break;
        }
    }
}
