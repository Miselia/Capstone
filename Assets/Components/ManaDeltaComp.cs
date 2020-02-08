using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct ManaDeltaComp : IComponentData
{
    public float delta;

    public ManaDeltaComp(float delta)
    {
        this.delta = delta;


    }
}