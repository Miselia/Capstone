using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct SpawnDelayComp : IComponentData
{
    public Vector2 movementVector; // direction and speed simultaneously
    public int timer; //Amount of time delayed
    public float radius;

    public SpawnDelayComp(Vector2 movementVector, int timer, float radius)
    {
        this.movementVector = movementVector;
        this.timer = timer;
        this.radius = radius;
    }
}
