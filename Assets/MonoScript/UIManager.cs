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
            player1Health.text = ("Health: " + ue.p1Health.ToString());
            player2Health.text = ("Health: " + ue.p2Health.ToString());
            player1Mana.text = ("Mana: " + ue.p1Mana.ToString());
            player2Mana.text = ("Mana: " + ue.p2Mana.ToString());

            return true;
        }
        return false;
    }
}
