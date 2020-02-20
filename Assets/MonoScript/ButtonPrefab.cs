using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPrefab : MonoBehaviour
{
    [SerializeField] private Text buttonText;
    public void SetText(string inputText)
    {
        buttonText.text = inputText;
    }

    public void Onclick()
    {

    }
}
