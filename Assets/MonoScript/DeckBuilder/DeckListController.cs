using Assets.Events.GenericEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckListController : MonoBehaviour, IGenericEventListener
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private DeckBuilderGame dbGame;
    void Start()
    {
        EventManager.instance.RegisterListener<AddCardtoDeckScrollListEvent>(this);

        /* 
         * Have the decklist content populate based on the Deck file that was loaded
         * into the DeckBuilderGame
         */

        List<CardData> cl = dbGame.GetCardLibrary();
        foreach (int id in dbGame.builderDeck.GetDeck())
        {
            AddButtontoDeckListUI(cl[id].cardID, cl[id].cardName);
        }
    }

    public void ButtonClicked(int id)
    {
        Debug.Log("Button added to DeckListController");
    }

    public bool HandleEvent(IGenericEvent evt)
    {
        if (evt is AddCardtoDeckScrollListEvent)
        {
            AddCardtoDeckScrollListEvent add = evt as AddCardtoDeckScrollListEvent;
            AddButtontoDeckListUI(add.cardID, add.cardName);
            return true;
        }
        return false;
    }

    private void AddButtontoDeckListUI(int id, string name)
    {
        GameObject button = Instantiate(buttonPrefab) as GameObject;
        button.SetActive(true);

        button.GetComponent<DeckButtonPrefab>().Initialize(name, id);
        button.transform.SetParent(buttonPrefab.transform.parent, false);
    }
}
