using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Assets.MonoScript;

public struct CollisionComponent : IComponentData
{
    public float collisionRadius;
    public float width;
    public byte mask;

    public CollisionComponent(float collisionRadius, float width, byte mask)
    {
        this.collisionRadius = collisionRadius;
        this.width = collisionRadius;
        this.mask = mask;
    }
}
