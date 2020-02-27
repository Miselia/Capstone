using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class DeckLobbyScript : MonoBehaviour
{
    // All new game options
    [SerializeField] private Dropdown factionChoices1;
    [SerializeField] private Dropdown factionChoices2;
    private string factionChoice1;
    private string factionChoice2;

    // All edit deck options
    [SerializeField] private Dropdown decksAvailable;

    public static Deck chosenDeck;
    List<string> allFileNames;

    public void Start()
    {
        /*
         * The Start() function will be used to dynamically define the options
         * In the deck loading section of the UI only, as the faction chioces
         * can simply be placed directly into the Dropdowns from the Unity editor
         */
        allFileNames = new List<string>();
        allFileNames.Add("None");

        decksAvailable.options.Clear();

        string[] allFilePaths = Directory.GetFiles(@"Assets/Resources/", "*.txt");
        for (int i=0; i < allFilePaths.Length; i++)
        {
            allFileNames.Add(allFilePaths[i].Replace("Assets/Resources/", "").Replace(".txt", ""));
        }

        decksAvailable.AddOptions(allFileNames);
    }

    public void NewDeckToDeckBuilder()
    {
        Debug.Log("New deck to deck builder clicked");
    }

    public void EditDeckToDeckBuilder()
    {
        Debug.Log("Edit deck to deck builder clicked");
    }
}
