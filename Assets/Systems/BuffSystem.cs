﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Assets.Resources;
using UnityEngine.SceneManagement;
using Unity.Transforms;

public class BuffSystem : ComponentSystem
{

    protected override void OnUpdate()
    {
        // Loop through all entities that are buffed
        // Whenever a buff is applied to an Entity it will also be given an "isBuffedComponent"
        Entities.ForEach((Entity e, ref IsBuffedComponent ibc) =>
        {
            // Mana Regen Buff
            if(EntityManager.HasComponent<ManaRegenBuffComp>(e) && EntityManager.HasComponent<PlayerComponent>(e))
            {
                EventManager.instance.QueueEvent(new SoundEvent("Other","BuffSelf"));
                ManaRegenBuffComp mbc = EntityManager.GetComponentData<ManaRegenBuffComp>(e);
                if (mbc.timer == mbc.maxTimer)
                {
                    PlayerComponent playComp = World.Active.EntityManager.GetComponentData<PlayerComponent>(e);
                    EventManager.instance.QueueEvent(new SoundEvent(2));

                    playComp.manaRegen += mbc.value;
                    World.Active.EntityManager.SetComponentData<PlayerComponent>(e, playComp);
                    Debug.Log("Playing ManaRegenSound");
                }

                mbc.timer--;
                World.Active.EntityManager.SetComponentData<ManaRegenBuffComp>(e, mbc);

                if (mbc.timer <= 0)
                {
                    Debug.Log("Mana Regen Expired");
                    PlayerComponent playComp = World.Active.EntityManager.GetComponentData<PlayerComponent>(e);

                    playComp.manaRegen = Constants.PlayerManaRegen;
                    EntityManager.RemoveComponent<ManaRegenBuffComp>(e);

                    if(!EntityManager.HasComponent<ManaRegenBuffComp>(e) &&
                       !EntityManager.HasComponent<ProjectileSpeedBuffComp>(e) &&
                       !EntityManager.HasComponent<ViperCurseComponent>(e))
                    {
                        EntityManager.RemoveComponent<IsBuffedComponent>(e);
                        Debug.Log("Entity is no longer Buffed");
                    }
                }
            }

            // Projectile Speed Buff
            if(EntityManager.HasComponent<ProjectileSpeedBuffComp>(e))
            {
                ProjectileSpeedBuffComp sbc = EntityManager.GetComponentData<ProjectileSpeedBuffComp>(e);

                if (sbc.timer == sbc.maxTimer)
                {
                    MovementComponent moveComp = EntityManager.GetComponentData<MovementComponent>(e);
                    EventManager.instance.QueueEvent(new SoundEvent(3));

                    sbc.original = moveComp.movementVector;
                    moveComp.movementVector *= sbc.value;
                    EntityManager.SetComponentData<MovementComponent>(e, moveComp);
                }

                sbc.timer--;
                World.Active.EntityManager.SetComponentData<ProjectileSpeedBuffComp>(e, sbc);

                if (sbc.timer <= 0)
                {
                    MovementComponent moveComp = EntityManager.GetComponentData<MovementComponent>(e);

                    moveComp.movementVector = sbc.original;
                    EntityManager.SetComponentData<MovementComponent>(e, moveComp);

                    if (!EntityManager.HasComponent<ManaRegenBuffComp>(e) &&
                       !EntityManager.HasComponent<ProjectileSpeedBuffComp>(e) &&
                       !EntityManager.HasComponent<ViperCurseComponent>(e))
                    {
                        EntityManager.RemoveComponent<IsBuffedComponent>(e);
                    }
                }
            }

            // Curse of the Viper
            if(EntityManager.HasComponent<ViperCurseComponent>(e))
            {
                ViperCurseComponent vcc = EntityManager.GetComponentData<ViperCurseComponent>(e);

                if(vcc.timer == vcc.maxTimer)
                {
                    // Optional sound effect, perhaps a voice line is played when the card is played (sound not played here)
                    // and the sound effect for the curse happens when the curse is received (sound is played here)
                    Debug.Log("Opponent recieved the Curse of the Viper");
                }

                if(vcc.timer % 100 == 0)
                {
                    // Code to spawn a projectile goes here, happens every 20 updates
                    Vector2 mov = new Vector2(EntityManager.GetComponentData<Translation>(e).Value.x, EntityManager.GetComponentData<Translation>(e).Value.y);
                    EventManager.instance.QueueEvent(new CreateProjectileEvent("Viper", Constants.DefaultProjectileDamage, mov, new Vector2(), 0.35f, 60));
                }

                vcc.timer--;
                World.Active.EntityManager.SetComponentData<ViperCurseComponent>(e, vcc);

                if (vcc.timer <= 0)
                {
                    EntityManager.RemoveComponent<ViperCurseComponent>(e);

                    if (!EntityManager.HasComponent<ManaRegenBuffComp>(e) &&
                       !EntityManager.HasComponent<ProjectileSpeedBuffComp>(e) &&
                       !EntityManager.HasComponent<ViperCurseComponent>(e))
                    {
                        EntityManager.RemoveComponent<IsBuffedComponent>(e);
                    }
                }
            }
        });
    }
}
