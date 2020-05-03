using UnityEngine;
using Unity.Entities;
using Assets.Resources;

public struct PlayerBoundaryComponent : IComponentData
{
    public Vector2 Normal;
    // Side = 1 if placed on Player 1's side
    // Side = 2 if placed on Player 2's side
    public int side;
    // Width set to Constants.PlayerBoundarySize, will need to be updated since constructor requires definition at compile time
    // whereas Constants.PlayerBoundarySize is a run-time "calculation"
    public int width;
    
    public PlayerBoundaryComponent( Vector2 normal, int side, int w = 13)
    {
        this.Normal = normal;
        this.side = side;
        this.width = w;
    }
}
