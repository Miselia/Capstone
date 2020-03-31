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
    [SerializeField] private Text info;
    [SerializeField] private DeckListController listControl;
    private string traits;
    private string flavor;
    public UnityEvent leftClick;
    public UnityEvent middleClick;
    public UnityEvent rightClick;
    public void SetText(string inputText)
    {
        buttonText.text = inputText;
    }
    public void Initialize(string cardName, int cardNum, string traits, string flavor)
    {
        buttonText.text = cardName;
        cardNumber = cardNum;
        this.traits = traits;
        this.flavor = flavor;
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
            info.text = (buttonText.text + "\n" + traits + "\n" + flavor);
        else if (eventData.button == PointerEventData.InputButton.Right)
            rightClick.Invoke();
    }
}
