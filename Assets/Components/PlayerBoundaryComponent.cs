using UnityEngine;
using Unity.Entities;

public struct PlayerBoundaryComponent : IComponentData
{
    public Vector2 Normal;
    // Side = 1 if placed on Player 1's side
    // Side = 2 if placed on Player 2's side
    public int side;
    
    public PlayerBoundaryComponent( Vector2 normal, int side )
    {
        this.Normal = normal;
        this.side = side;
    }
}
