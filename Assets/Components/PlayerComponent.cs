using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct PlayerComponent : IComponentData
{
    // Things we may need
    // An "Input Method" if such exists in Unity already
    public int playerID;
    public int healthRemaining;

    public PlayerComponent(int playerID, int maximumHealth)
    {
        this.playerID = playerID;
        this.healthRemaining = maximumHealth;
    }
}
