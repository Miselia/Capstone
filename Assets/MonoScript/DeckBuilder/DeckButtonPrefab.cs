using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class DeckButtonPrefab : MonoBehaviour, IPointerClickHandler
{
    private int cardNumber;
    [SerializeField] private Text buttonText;
    [SerializeField] private DeckListController listControl;
    public UnityEvent leftClick;
    public UnityEvent middleClick;
    public UnityEvent rightClick;
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
    public void OnPrefabRightClicked()
    {
        listControl.ButtonRightClicked(cardNumber);
        Object.Destroy(this.gameObject);
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
