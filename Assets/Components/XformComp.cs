using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct XformComponent : IComponentData
{
    public boolean hasXform; // transform flag
}