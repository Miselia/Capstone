using Unity.Entities;
using UnityEngine;

public struct ProjectileBoundaryComponent : IComponentData
{
    public Vector2 Normal;

    public ProjectileBoundaryComponent( Vector2 normal )
    {
        this.Normal = normal;
    }
}
