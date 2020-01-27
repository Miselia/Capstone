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
        Debug.Log("p1: " + p1DeckDropdown.value);
        Debug.Log("p2: " + p2DeckDropdown.value);

        if(p1DeckDropdown.GetComponent<Dropdown>().value != 0 &&
            p2DeckDropdown.GetComponent<Dropdown>().value != 0)
        {
            /** TODO:
             *    Make the GameScene transition actually work
             *    Pass in the two different deck files into the GameScene
             */
            SceneManager.LoadScene("GameScene");
        }
    }
}
