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

        string[] allFilePaths = Directory.GetFiles(@"Assets/Resources/ValidDecks/", "*.txt");

        // The lobby script will only check the ValidDecks folder to display to the players
        for (int i = 0; i < allFilePaths.Length; i++)
        {
            allFileNames.Add(allFilePaths[i].Replace("Assets/Resources/ValidDecks/", "").Replace(".txt", ""));
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
            p1Deck = new Deck("ValidDecks/" + p1DeckString);
            p2Deck = new Deck("ValidDecks/" + p2DeckString);
            Debug.Log("p1DeckString = " + p1DeckString);

            if (p1Deck != null && p2Deck != null && p1Deck.CheckDeckIsValid() && p2Deck.CheckDeckIsValid())
            {
                SceneManager.LoadScene("GameScene");
            }
            else
            {
                Debug.Log("One or both of the decks chosen has either corrupted or been tampered with (you know what you've done)" +
                            "Go back to the deckbuilder and click 'Save' to re-create the deck");
                // TODO: Write code to delete the decks from the folders and remove the options from the dropdowns
                // This is low priority, and can be done after capstone
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
