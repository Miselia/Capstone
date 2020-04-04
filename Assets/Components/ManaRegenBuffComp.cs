using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct ManaRegenBuffComp : IComponentData
{
    public float value;
    public float timer;
    public float maxTimer;

    public ManaRegenBuffComp(float value, float timer, float maxTimer)
    {
        this.value = value;
        this.timer = timer;
        this.maxTimer = maxTimer;
    }
}