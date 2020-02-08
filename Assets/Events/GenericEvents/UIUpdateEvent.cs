using Unity.Entities;
using UnityEngine;

public class UIUpdateEvent : IGenericEvent
{
    public int pHealth;
    public int pMana;
    public int pID;

    public UIUpdateEvent(int pHealth, int pMana, int pID)
    {
        this.pHealth = pHealth;
        this.pMana = pMana;
        this.pID = pID;

        
    }
}
