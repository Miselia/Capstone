using System.Collections;
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
        /*Entities.ForEach((Entity e, ref GenericBuffComponent buff) =>
        {
            if(buff.timer == buff.maxTimer)
            {
                // So long as buffType value = its corresponding sound listener index we can use it to generate the correct sound event
                EventManager.instance.QueueEvent(new SoundEvent(buff.buffType));
            }

            switch(buff.buffType)
            {
                // Mana Regen Buff
                case 2:
                    if (EntityManager.HasComponent<PlayerComponent>(e))
                    {
                        PlayerComponent playComp = World.Active.EntityManager.GetComponentData<PlayerComponent>(e);
                        playComp.manaRegen += buff.value;
                        World.Active.EntityManager.SetComponentData<PlayerComponent>(e, playComp);
                    }
                    break;
                // Projectile Speed Buff
                case 3:
                    if (EntityManager.HasComponent<ProjectileComponent>(e))
                    {
                        MovementComponent moveComp = EntityManager.GetComponentData<MovementComponent>(e);
                        moveComp.movementVector *= buff.value;
                    }
                    break;
                // Viper Curse Buff
                case 4:
                    /*if (EntityManager.HasComponent<PlayerComponent>(e))
                    {

                    }
                    break;
            }

            if(buff.timer <= 0)
            {
                switch(buff.buffType)
                {
                    case 2:
                        PlayerComponent playComp = World.Active.EntityManager.GetComponentData<PlayerComponent>(e);
                        playComp.manaRegen = Constants.PlayerManaRegen;
                        EntityManager.RemoveComponent<>
                }
            }
        });*/

        // Loop through all entities that are buffed
        // Whenever a buff is applied to an Entity it will also be given an "isBuffedComponent"
        Entities.ForEach((Entity e, ref IsBuffedComponent ibc) =>
        {
            if(EntityManager.HasComponent<ManaRegenBuffComp>(e) && EntityManager.HasComponent<PlayerComponent>(e))
            {
                ManaRegenBuffComp mbc = EntityManager.GetComponentData<ManaRegenBuffComp>(e);
                if (mbc.timer == mbc.maxTimer)
                {
                    PlayerComponent playComp = World.Active.EntityManager.GetComponentData<PlayerComponent>(e);
                    EventManager.instance.QueueEvent(new SoundEvent(2));

                    playComp.manaRegen += mbc.value;
                    World.Active.EntityManager.SetComponentData<PlayerComponent>(e, playComp);
                }

                if(mbc.timer <= 0)
                {
                    PlayerComponent playComp = World.Active.EntityManager.GetComponentData<PlayerComponent>(e);

                    playComp.manaRegen = Constants.PlayerManaRegen;
                    EntityManager.RemoveComponent<ManaRegenBuffComp>(e);

                    if(!EntityManager.HasComponent<ManaRegenBuffComp>(e) &&
                       !EntityManager.HasComponent<ProjectileSpeedBuffComp>(e) &&
                       !EntityManager.HasComponent<ViperCurseComponent>(e))
                    {
                        EntityManager.RemoveComponent<IsBuffedComponent>(e);
                    }
                
                }
                mbc.timer--;
            }

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

                if(sbc.timer <= 0)
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
                sbc.timer--;
            }

            if(EntityManager.HasComponent<ViperCurseComponent>(e))
            {
                ViperCurseComponent vcc = EntityManager.GetComponentData<ViperCurseComponent>(e);

                if(vcc.timer == vcc.maxTimer)
                {
                    // Optional sound effect, perhaps a voice line is played when the card is played (sound not played here)
                    // and the sound effect for the curse happens when the curse is received (sound is played here)
                }

                if(vcc.timer % 20 == 0)
                {
                    // Code to spawn a projectile goes here, happens every 20 updates
                    Vector2 mov = new Vector2(EntityManager.GetComponentData<Translation>(e).Value.x, EntityManager.GetComponentData<Translation>(e).Value.y);
                    EventManager.instance.QueueEvent(new CreateProjectileEvent("Viper", Constants.DefaultProjectileDamage, mov, new Vector2(), 0.35f, 60));
                }

                if(vcc.timer <= 0)
                {
                    EntityManager.RemoveComponent<ViperCurseComponent>(e);

                    if (!EntityManager.HasComponent<ManaRegenBuffComp>(e) &&
                       !EntityManager.HasComponent<ProjectileSpeedBuffComp>(e) &&
                       !EntityManager.HasComponent<ViperCurseComponent>(e))
                    {
                        EntityManager.RemoveComponent<IsBuffedComponent>(e);
                    }
                }
                vcc.timer--;
            }
        });
        /*
        //ManaRegenBuff
        Entities.ForEach((Entity e, ref PlayerComponent p, ref ManaRegenBuffComp mb) =>
        {
            if (mb.timer == mb.maxTimer)
            {
                EventManager.instance.QueueEvent(new SoundEvent(2));
            }
            p.manaRegen = p.manaRegen+ mb.value;
            mb.value = 0;
            
            if (mb.timer <= 0)
            {
                p.manaRegen = Constants.PlayerManaRegen;
                World.Active.EntityManager.RemoveComponent<ManaRegenBuffComp>(e);
            }
            mb.timer--;
        });

        //ProjectileSpeedBuff
        Entities.ForEach((Entity e, ref ProjectileComponent p, ref ProjectileProjectileSpeedBuffComp psb, ref MovementComponent m) =>
        {
            if(psb.timer == psb.maxTimer)
            {
                EventManager.instance.QueueEvent(new SoundEvent(3));
                psb.original = m.movementVector;
                m.movementVector = m.movementVector * psb.value;
            }

            if (psb.timer <= 0)
            {
                m.movementVector = psb.original;
                World.Active.EntityManager.RemoveComponent<ProjectileProjectileSpeedBuffComp>(e);
            }
            psb.timer--;
        });

        // Curse of the Viper
        Entities.ForEach((Entity e, ref PlayerComponent, ref ViperCurseComponent) =>
        {

        });
        */
    }
}
