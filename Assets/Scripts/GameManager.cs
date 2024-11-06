using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float health;
    public float maxHealth;
    public float magic;
    public float maxMagic;

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

        maxHealth = 500f;
        maxMagic = 10f;

        health = maxHealth;
        magic = 0f;

        //AudioManager.Instance.InitVolumes(50, 50);    // 나중에 옵션 값 가져오기
        //AudioManager.Instance.PlaySound2D("TitleScreen", 0, true, AudioManager.AudioType.BGM);
    }

    
    void Update()
    {
        
    }
}
