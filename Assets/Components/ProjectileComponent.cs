using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct ProjectileComponent : IComponentData
{
    public int bouncesLeft; // Number of bounces, if any

    public ProjectileComponent(int maximumBounces)
    {
        this.bouncesLeft = maximumBounces;
    }
}
