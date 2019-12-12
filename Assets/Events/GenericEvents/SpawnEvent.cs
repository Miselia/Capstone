using Unity.Entities;
using UnityEngine;

public class SpawnEvent : IGenericEvent
{
    public int player;
    public int cardID;

    public SpawnEvent(int player, int cardID)
    {
        this.player = player;
        this.cardID = cardID;

        Debug.Log("Spawn Event Created");
    }
}
