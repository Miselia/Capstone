using Assets.Events.GenericEvents;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
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

    public void ButtonClicked(int id)
    {

        //EventManager.eventManager.QueueEvent(new SpawnEvent(
        //    new CardEntity.Create(World.Active.EntityManager, new Vector2(), id, 0, 1, ))
        foreach(CardData cd in dbGame.GetCardLibrary())
        {
            if (cd.cardID == id)
                dbGame.AddCardtoHand(id);
            //CardEntity.Create(World.Active.EntityManager, new Vector2(), cd.cardID, 1, 1, cd.manaCost, dbGame.mesh2D, cd.getMaterial());
        }
    }
    public void ButtonRightClicked(int id)
    {
        Debug.Log("Detect RightClick");
        Debug.Log(dbGame.RemoveCardFromDeck(id));

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
