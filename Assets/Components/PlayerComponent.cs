using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct PlayerComponent : IComponentData
{
    // Things we may need
    // An "Input Method" if such exists in Unity already
    public string playerName;
    public int healthRemaining;

    public PlayerComponent(string playerName, int maximumHealth)
    {
        this.playerName = playerName;
        this.healthRemaining = maximumHealth;
    }
}
