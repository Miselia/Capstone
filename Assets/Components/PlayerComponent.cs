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
    public float maxMana;
    public float manaRegen;
    public float mana;

    public PlayerComponent(int playerID, int maximumHealth, float maxMana, float manaRegen)
    {
        this.playerID = playerID;
        this.healthRemaining = maximumHealth;
        this.maxMana = maxMana;
        this.manaRegen = manaRegen;
        this.mana = maxMana;
        EventManager.instance.QueueEvent(new UIUpdateEvent(healthRemaining, (int)Mathf.Floor(mana), playerID));

        Debug.Log("Constructor being called for player");
    }

    public int[] LoseHealth(int damage)
    {
        healthRemaining -= damage;
        return new int[] { healthRemaining, (int)Mathf.Floor(mana), playerID };
    }
    public int[] adjustMana(float delta)
    {
        Debug.Log("Player Mana was at " + mana);
        mana += delta;
        Debug.Log("Player Mana Adjusted to" + mana);
        int tempMana = (int)Mathf.Floor(mana);
        Debug.Log("PlayerMana casted to: " + tempMana);
        return new int[] { healthRemaining, tempMana, playerID };
    }
    
    public int[] setMana(float value)
    {
        mana = value;
        Debug.Log("Player Mana Set to" + mana);
        return new int[] { healthRemaining,(int) Mathf.Floor(mana), playerID };
    }
    
}
