using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IGenericEventListener
{
    [SerializeField] private Text player1Health;
    [SerializeField] private Text player1Mana;
    [SerializeField] private Text player2Health;
    [SerializeField] private Text player2Mana;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.RegisterListener<UIUpdateEvent>(this);
    }

    // Update is called once per frame
    public bool HandleEvent(IGenericEvent evt)
    {
        if (evt is UIUpdateEvent)
        {
            UIUpdateEvent ue = (UIUpdateEvent) evt;
            Debug.Log("UI Update Event is handled");
            if (ue.pID == 1)
            {
                player1Health.text = ("Health: " + ue.pHealth.ToString());
                player1Mana.text = ("Mana: " + ue.pMana.ToString());
                Debug.Log("UI Updated Player 1");
            }
            if (ue.pID == 2)
            {
                player2Health.text = ("Health: " + ue.pHealth.ToString());
                player2Mana.text = ("Mana: " + ue.pMana.ToString());
                Debug.Log("UI Updated Player 2");
            }

            return true;
        }
        return false;
    }
}