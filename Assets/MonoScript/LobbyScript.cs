using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyScript : MonoBehaviour
{
    [SerializeField] private Dropdown p1DeckDropdown;
    [SerializeField] private Dropdown p2DeckDropdown;
    List<string> allFileNames;
    public static string p1DeckString;
    public static string p2DeckString;
    public static Deck p1Deck;
    public static Deck p2Deck;

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
            p1DeckString = allFileNames[p1DeckDropdown.value];
            p2DeckString = allFileNames[p2DeckDropdown.value];
            p1Deck = new Deck(p1DeckString);
            p2Deck = new Deck(p2DeckString);

            if (p1Deck != null && p2Deck != null)
            {
                SceneManager.LoadScene("GameScene");
            }
        }
        else
            Debug.Log("Decks not valid, try again");
    }
    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
