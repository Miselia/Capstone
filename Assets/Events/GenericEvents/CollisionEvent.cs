using Unity.Entities;
using UnityEngine;

public class CollisionEvent : IGenericEvent
{
    public Entity entityA;
    public Entity entityB;

    public CollisionEvent(Entity entityA, Entity entityB)
    {
        this.entityA = entityA;
        this.entityB = entityB;

        //Debug.Log("Collision Event Created");
    }
}
