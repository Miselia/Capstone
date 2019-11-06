using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct XformComponent : IComponentData
{
    public bool hasXform; // transform flag

    public XformComponent(bool hasXform)
    {
        this.hasXform = hasXform;
    }
}