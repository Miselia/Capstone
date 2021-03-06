﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct SpawnDelayComp : IComponentData
{
    public Vector2 movementVector; // direction and speed simultaneously
    public float timer; //Amount of time delayed
    public float radius;
    public byte mask;
    public float extraScale;

    public SpawnDelayComp(Vector2 movementVector, float timer, float radius, byte mask, float extraScale)
    {
        this.movementVector = movementVector;
        this.timer = timer;
        this.radius = radius;
        this.mask = mask;
        this.extraScale = extraScale;
    }
}
