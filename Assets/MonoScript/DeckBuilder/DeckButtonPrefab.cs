using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckButtonPrefab : MonoBehaviour
{
    private int cardNumber;
    [SerializeField] private Text buttonText;
    [SerializeField] private DeckListController listControl;
    public void SetText(string inputText)
    {
        buttonText.text = inputText;
    }
    public void Initialize(string cardName, int cardNum)
    {
        buttonText.text = cardName;
        cardNumber = cardNum;
    }

    public void OnPrefabClicked()
    {
        listControl.ButtonClicked(cardNumber);
    }
}
