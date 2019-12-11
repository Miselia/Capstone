using Unity.Entities;
using UnityEngine;

public class SpawnEvent : IGenericEvent
{
    public Entity entityA;
    public Entity entityB;

    public SpawnEvent(Entity entityA, Entity entityB)
    {
        this.entityA = entityA;
        this.entityB = entityB;

        //Debug.Log("Collision Event Created");
    }
}
