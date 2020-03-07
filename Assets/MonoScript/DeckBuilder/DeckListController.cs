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
        EventManager.instance.RegisterListener<InitializeDeckBuilerDeckUIEvent>(this);
    }
    private void InitializationFunction()
    {
        /* 
         * Have the decklist content populate based on the Deck file that was loaded
         * into the DeckBuilderGame
         */
        Debug.Log("WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW");
        // "cl" contains one copy of each unique card data found in the deck
        List<CardData> cl = dbGame.GetCardLibrary();

        foreach (int id in dbGame.builderDeck.GetDeck())
        {
            foreach(CardData cd in cl)
            {
                if (cd.cardID == id)
                    AddButtontoDeckListUI(cd.cardID, cd.cardName);
            }
        }
    }

    // We may want to implement this before presentation
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
        if (evt is InitializeDeckBuilerDeckUIEvent)
        {
            Debug.Log("KKKKKKKKKKKKKKKKKKKKKKKKKKKK");
            InitializationFunction();
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
