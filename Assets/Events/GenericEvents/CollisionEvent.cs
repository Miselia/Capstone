using Unity.Entities;
using UnityEngine;

public class CollisionEvent : IGenericEvent
{
    public Entity entityA;
    public Entity entityB;
    public int collisionMask;

    public CollisionEvent(Entity entityA, Entity entityB, int mask)
    {
        this.entityA = entityA;
        this.entityB = entityB;
        collisionMask = mask;
        //Debug.Log("Collision Event Created");
    }
}
