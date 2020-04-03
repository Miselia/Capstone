using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct ProjectileSpeedBuffComp : IComponentData
{
    public float value;
    public float timer;
    public float maxTimer;
    public Vector2 original;

    public ProjectileSpeedBuffComp(float value, float timer, Vector2 original)
    {
        this.value = value;
        this.timer = timer;
        this.maxTimer = timer;
        this.original = original;
    }
}