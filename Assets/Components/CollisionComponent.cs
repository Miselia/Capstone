using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct CollisionComponent : IComponentData
{
    public float collisionRadius;

    public CollisionComponent(float collisionRadius)
    {
        this.collisionRadius = collisionRadius;
    }
}
