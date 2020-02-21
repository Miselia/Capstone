using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonListController : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    CardLibrary cl;
    void Start()
    {
        cl = (CardLibrary)GameObject.Find("CardLibrary").GetComponent("CardLibrary");

        foreach(CardData data in cl.GetListByName(0))
        {
            GameObject button = Instantiate(buttonPrefab) as GameObject;
            button.SetActive(true);

            button.GetComponent<CardButtonPrefab>().Initialize(data.cardName, data.cardID);
            button.transform.SetParent(buttonPrefab.transform.parent, false);
        }
    }

    public void ButtonClicked(int id)
    {
        
    }
}
