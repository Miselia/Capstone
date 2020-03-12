using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CardButtonPrefab : MonoBehaviour, IPointerClickHandler
{
    private int cardNumber;
    [SerializeField] private Text buttonText;
    [SerializeField] private ButtonListController listControl;
    public UnityEvent leftClick;
    public UnityEvent middleClick;
    public UnityEvent rightClick;
    public void SetText(string inputText)
    {
        buttonText.text = inputText;
    }
    public void Initialize(string cardName, int cardNum, Material cardMat)
    {
        buttonText.text = cardName;
        cardNumber = cardNum;
        gameObject.GetComponent<Image>().material = cardMat;
    }

    public void OnPrefabClicked()
    {
        listControl.ButtonClicked(cardNumber, buttonText.text);
    }
        

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            leftClick.Invoke();
        else if (eventData.button == PointerEventData.InputButton.Middle)
            middleClick.Invoke();
        else if (eventData.button == PointerEventData.InputButton.Right)
            rightClick.Invoke();
    }
    
}
