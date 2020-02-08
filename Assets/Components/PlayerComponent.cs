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
    public int maxMana;
    public int mana;

    public PlayerComponent(int playerID, int maximumHealth, int maxMana)
    {
        this.playerID = playerID;
        this.healthRemaining = maximumHealth;
        this.maxMana = maxMana;
        this.mana = maxMana;
    }

    public int[] LoseHealth(int damage)
    {
        healthRemaining -= damage;
        return new int[] { healthRemaining, mana, playerID };
    }
}
