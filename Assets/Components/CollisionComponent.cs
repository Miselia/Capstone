using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct CollisionComponent : IComponentData
{
    public float collisionRadius;
    public float width;

    public CollisionComponent(float collisionRadius, float width)
    {
        this.collisionRadius = collisionRadius;
        this.width = collisionRadius;
    }
}
