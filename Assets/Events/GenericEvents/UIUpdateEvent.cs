using Unity.Entities;
using UnityEngine;

public class UIUpdateEvent : IGenericEvent
{
    public int p1Health;
    public int p2Health;
    public int p1Mana;
    public int p2Mana;

    public UIUpdateEvent(int p1Health, int p2Health, int p1Mana, int p2Mana)
    {
        this.p1Health = p1Health;
        this.p2Health = p2Health;
        this.p1Mana = p1Mana;
        this.p2Mana = p2Mana;

        Debug.Log("UI Updated");
    }
}
