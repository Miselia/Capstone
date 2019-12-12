using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class EndCollisionEvent : IGenericEvent
{
    public Entity entityA;
    public Entity entityB;

    public EndCollisionEvent(Entity entityA, Entity entityB)
    {
        this.entityA = entityA;
        this.entityB = entityB;
    }
}
