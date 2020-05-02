using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct MovementSpeedBuffComp : IComponentData
{
    public float value;
    public float timer;
    public float maxTimer;

    public MovementSpeedBuffComp(float value, float timer)
    {
        this.value = value;
        this.timer = timer;
        this.maxTimer = timer;
    }
}