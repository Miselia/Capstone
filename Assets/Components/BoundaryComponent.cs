using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct BoundaryComponent : IComponentData
{
    public Vector2 Normal;
    
    public BoundaryComponent( Vector2 normal )
    {
        this.Normal = normal;
    }
}
