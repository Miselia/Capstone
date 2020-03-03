using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeckLobbyScript : MonoBehaviour
{
    // All new deck options
    [SerializeField] private Dropdown factionChoicesDropdown1;
    [SerializeField] private Dropdown factionChoicesDropdown2;
    List<string> allFactionNames;
    private string factionChoice1;
    private string factionChoice2;

    // All edit deck options
    [SerializeField] private Dropdown decksAvailable;
    List<string> allFileNames;
    private string loadedDeck;

    public static Deck chosenDeck;

    public void Start()
    {
        // The section initializes the Decks Available choice (load deck from file) dropdown
        allFileNames = new List<string>();
        allFileNames.Add("None");

        decksAvailable.options.Clear();

        string[] allFilePaths = Directory.GetFiles(@"Assets/Resources/", "*.txt");
        for (int i=0; i < allFilePaths.Length; i++)
        {
            allFileNames.Add(allFilePaths[i].Replace("Assets/Resources/", "").Replace(".txt", ""));
        }

        decksAvailable.AddOptions(allFileNames);

        // This section initializes the Faction choice (new deck creation) dropdowns
        allFactionNames = new List<string>();
        allFactionNames.Add("None");

        factionChoicesDropdown1.options.Clear();
        factionChoicesDropdown2.options.Clear();

        allFactionNames.Add("Fantasy");
        allFactionNames.Add("Steampunk");
        allFactionNames.Add("Horror");
        allFactionNames.Add("Sci-Fi");

        factionChoicesDropdown1.AddOptions(allFactionNames);
        factionChoicesDropdown2.AddOptions(allFactionNames);
    }

    public void NewDeckToDeckBuilder()
    {
        Debug.Log("New deck to deck builder clicked");
        if (factionChoicesDropdown1.value != 0 && factionChoicesDropdown2.value != 0)
        {
            factionChoice1 = allFactionNames[factionChoicesDropdown1.value];
            factionChoice2 = allFactionNames[factionChoicesDropdown2.value];

            Debug.Log("Created Deck: " + factionChoice1);
            chosenDeck = new Deck(factionChoice1, factionChoice2);
            SceneManager.LoadScene("DeckBuilder");
        }
    }

    public void EditDeckToDeckBuilder()
    {
        Debug.Log("Edit deck to deck builder clicked");
        if (decksAvailable.value != 0)
        {
            loadedDeck = allFileNames[decksAvailable.value];

            Debug.Log("Loaded deck " + loadedDeck + ".txt");
            chosenDeck = new Deck(loadedDeck + ".txt");
            SceneManager.LoadScene("DeckBuilder");
        }
    }
    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
