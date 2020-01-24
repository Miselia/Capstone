using UnityEngine;
using Unity.Entities;

public struct PlayerBoundaryComponent : IComponentData
{
    public Vector2 Normal;
    
    public PlayerBoundaryComponent( Vector2 normal )
    {
        this.Normal = normal;
    }
}
