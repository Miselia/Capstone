using Unity.Entities;
using UnityEngine;

public class SpawnEvent : IGenericEvent
{
    public Entity card;
    public Entity player;
    public Entity opponent;

    public SpawnEvent(Entity card, Entity player, Entity opponent)
    {
        this.card = card;
        this.player = player;
        this.opponent = opponent;
        //Debug.Log("Spawn Event Created");
    }
}
