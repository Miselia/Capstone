using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonListController : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    void Start()
    {
        CardLibrary cl = (CardLibrary)GameObject.Find("CardLibrary").GetComponent("CardLibrary");

        foreach(CardData data in cl.GetListByName(0))
        {
            GameObject button = Instantiate(buttonPrefab) as GameObject;
            button.SetActive(true);

            button.GetComponent<ButtonPrefab>().SetText(data.getName());
            button.transform.SetParent(buttonPrefab.transform.parent, false);
        }
    }
}
