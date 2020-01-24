using Unity.Entities;
using UnityEngine;

public class SpawnEvent : IGenericEvent
{
    public Entity card;
    public Entity player;

    public SpawnEvent(Entity card, Entity player)
    {
        this.card = card;
        this.player = player;

        Debug.Log("Spawn Event Created");
    }
}
