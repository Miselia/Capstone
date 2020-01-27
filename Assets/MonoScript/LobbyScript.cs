using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyScript : MonoBehaviour
{
    //void Start() { }
    //void Update() { }

    [SerializeField] private Dropdown p1DeckDropdown;
    [SerializeField] private Dropdown p2DeckDropdown;
    List<string> allFileNames;
    public static string p1Deck;
    public static string p2Deck;

    private void Start()
    {
        allFileNames = new List<string>();
        allFileNames.Add("None");

        p1DeckDropdown.options.Clear();
        p2DeckDropdown.options.Clear();

        string[] allFilePaths = Directory.GetFiles(@"Assets/Resources/", "*.txt");

        for (int i = 0; i < allFilePaths.Length; i++)
        {
            allFileNames.Add(allFilePaths[i].Replace("Assets/Resources/", "").Replace(".txt", ""));
        }

        p1DeckDropdown.AddOptions(allFileNames);
        p2DeckDropdown.AddOptions(allFileNames);
    }

    public void ChangeToGameScene()
    {
        if(p1DeckDropdown.value != 0 &&
           p2DeckDropdown.value != 0)
        {
            /** TODO:
             *    Make the GameScene transition actually work
             *    Pass in the two different deck files into the GameScene
             *      Option 1: Store values in 'Object' type and call DontDestroyOnLoad(Object)
             *      Option 2: Make these two values static, as static variables are always visible
             */

            p1Deck = allFileNames[p1DeckDropdown.value];
            p2Deck = allFileNames[p2DeckDropdown.value];
            //SceneManager.LoadScene("GameScene");
        }
    }
}
