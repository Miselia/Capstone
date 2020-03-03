using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Resources;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour, IGenericEventListener
{
    [SerializeField] private Text player1Health;
    [SerializeField] private Text player1Mana;
    [SerializeField] private Text player2Health;
    [SerializeField] private Text player2Mana;

    [SerializeField] private Bar player1HealthBar;
    [SerializeField] private Bar player1ManaBar;
    [SerializeField] private Bar player2HealthBar;
    [SerializeField] private Bar player2ManaBar;
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
                player1Health.text = (ue.pHealth.ToString());
                player1Mana.text = (ue.pMana.ToString());
                Debug.Log("UI Updated Player 1");
                player1HealthBar.SetSize(((float) ue.pHealth)/ Constants.PlayerMaximumHealth);
                player1ManaBar.SetSize((ue.pMana) / Constants.PlayerMaximumMana);

            }
            if (ue.pID == 2)
            {
                player2Health.text = (ue.pHealth.ToString());
                player2Mana.text = (ue.pMana.ToString());
                Debug.Log("UI Updated Player 2");

                player2HealthBar.SetSize(((float) ue.pHealth) / Constants.PlayerMaximumHealth);
                player2ManaBar.SetSize((ue.pMana) / Constants.PlayerMaximumMana);
            }

            return true;
        }
        return false;
    }
    public void Back()
    {
        SceneManager.LoadScene("DeckBuilderLobby");
    }
}