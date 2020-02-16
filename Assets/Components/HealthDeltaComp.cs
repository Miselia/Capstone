using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct HealthDeltaComp : IComponentData
{
    public int delta;

    public HealthDeltaComp(int delta)
    {
        this.delta = delta;


    }
}
