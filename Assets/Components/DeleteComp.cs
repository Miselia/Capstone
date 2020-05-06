using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct DeleteComp : IComponentData
{
    public float buffer;

    public DeleteComp(float buffer)
    {
        this.buffer = buffer;
    }
}