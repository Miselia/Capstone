﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Assets.Resources;
using UnityEngine.SceneManagement;

public class PlayerValueSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        if (SceneManager.GetActiveScene().name.Equals("GameScene") || SceneManager.GetActiveScene().name == "DeckBuilder")
        {
            Entities.ForEach((Entity e, ref PlayerComponent p, ref ManaDeltaComp delta) =>
            {

                p.mana = p.mana + delta.delta;
                World.Active.EntityManager.RemoveComponent<ManaDeltaComp>(e);

            });
            Entities.ForEach((Entity e, ref PlayerComponent p, ref HealthDeltaComp delta) =>
            {

                p.healthRemaining = p.healthRemaining + delta.delta;
                World.Active.EntityManager.RemoveComponent<HealthDeltaComp>(e);

            });
            Entities.ForEach((ref PlayerComponent p) =>
            {
                p.mana = p.mana + p.manaRegen;

                if (p.mana > p.maxMana)
                {
                    p.mana = p.maxMana;
                }

                EventManager.instance.QueueEvent(new UIUpdateEvent(p.healthRemaining, (int)Mathf.Floor(p.mana), p.playerID));

            });
        }
    }
}