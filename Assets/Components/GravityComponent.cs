using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct GravityComponent : IComponentData
{
    public float radius; // direction and speed simultaneously
    public float strength;

    public GravityComponent(float radius, float strength)
    {
        this.radius = radius;
        this.strength = strength;
    }
}
