using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct ManaRegenBuffComp : IComponentData
{
    public float value;
    public float timer;

    public ManaRegenBuffComp(float value, float timer)
    {
        this.value = value;
        this.timer = timer;
    }
}