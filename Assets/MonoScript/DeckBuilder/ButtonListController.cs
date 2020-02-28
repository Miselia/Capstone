using Assets.Events.GenericEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonListController : MonoBehaviour, IGenericEventListener
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

        EventManager.instance.RegisterListener<AddCardtoDeckScrollListEvent>(this);
    }

    public void ButtonClicked(int id, string cardName)
    {
        Debug.Log("Card attempt add to deck");
        EventManager.instance.QueueEvent(new AddCardtoDeckEvent(id, cardName));
    }

    public bool HandleEvent(IGenericEvent evt)
    {
        if (evt is AddCardtoDeckScrollListEvent)
        {
            AddCardtoDeckScrollListEvent add = evt as AddCardtoDeckScrollListEvent;
            Debug.Log("Name " + add.cardName + " ID: " + add.cardID + " added to deck list");
            return true;
        }
        return false;
    }
}
