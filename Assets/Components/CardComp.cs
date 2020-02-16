using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct CardComp : IComponentData
{
    public int cardID;
    public int cardSlot;
    public int player;
    public float manaCost;

    public CardComp(int cardID, int cardSlot, int player, float manaCost)
    {
        this.cardID = cardID;
        this.cardSlot = cardSlot; //0 is unselected
        this.player = player;
        this.manaCost = manaCost;
        
        
    }
}
