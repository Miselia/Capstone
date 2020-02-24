using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonListController : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private DeckBuilderGame dbGame;
    void Start()
    {
        List<CardData> cl = dbGame.GetCardLibrary();

        foreach(CardData data in cl)
        {
            GameObject button = Instantiate(buttonPrefab) as GameObject;
            button.SetActive(true);

            button.GetComponent<CardButtonPrefab>().Initialize(data.getName(), data.getID(), data.getMaterial()) ;
            button.transform.SetParent(buttonPrefab.transform.parent, false);
        }
    }

    public void ButtonClicked(int id)
    {
        string debug = dbGame.AddCardToDeck(id) ? "successfully" : "unsuccessfully";
        Debug.Log("Card added to deck " + debug);
    }
}
