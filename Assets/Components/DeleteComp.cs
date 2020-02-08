using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct DeleteComp : IComponentData
{
    public int buffer;

    public DeleteComp(int buffer)
    {
        this.buffer = buffer;


    }
}