using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardButtonPrefab : MonoBehaviour
{
    private int cardNumber;
    [SerializeField] private Text buttonText;
    [SerializeField] private ButtonListController listControl;
    public void SetText(string inputText)
    {
        buttonText.text = inputText;
    }
    public void Initialize(string cardName, int cardNum)
    {
        buttonText.text = cardName;
        cardNumber = cardNum;
    }

    public void Onclick()
    {
        listControl.ButtonClicked(cardNumber);
    }
}
