using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "Keybindings", menuName = "Keybindings")]
public class Keybindings : ScriptableObject
{
    public KeyCode Up1, Left1, Right1, Down1, CardA1, CardB1, CardC1, CardD1, Up2, Left2, Right2, Down2, CardA2, CardB2, CardC2, CardD2;

    public KeyCode CheckKey(string key)
    {
        switch (key)
        {
            case "Up1":
                return Up1;
            case "Left1":
                return Left1;
            case "Down1":
                return Down1;
            case "Right1":
                return Right1;
            case "CardA1":
                return CardA1;
            case "CardB1":
                return CardB1;
            case "CardC1":
                return CardC1;
            case "CardD1":
                return CardD1;
            case "Up2":
                return Up2;
            case "Left2":
                return Left2;
            case "Down2":
                return Down2;
            case "Right2":
                return Right2;
            case "CardA2":
                return CardA2;
            case "CardB2":
                return CardB2;
            case "CardC2":
                return CardC2;
            case "CardD2":
                return CardD2;
            
            default:
                return KeyCode.None;
        }
    }
}
