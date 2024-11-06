using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public enum GameTextType { Single, Two, Versus, Option, Credits, Quit }

    public enum OptionTextType { KeyConfig = 13, SaveReset, Default, Back}

    public enum GameOptionType { Continue, GoTitle, Option, Exit }

    public enum InfoType { Health, Magic, Coin, HearthStone, Enemy }
          
    [Header("# Option Info #")]
    GameTextType gameType;
    OptionTextType optionTextType;

    [Header("# Game Info #")]
    GameOptionType gameOptionType;
    InfoType infoType;
}
