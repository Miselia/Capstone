using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct CardComp : IComponentData
{
    public int cardID;
    public int cardSlot;
    public int player;

    public CardComp(int cardID, int cardSlot, int player)
    {
        this.cardID = cardID;
        this.cardSlot = cardSlot; //0 is unselected
        this.player = player;
        
        
    }
}
