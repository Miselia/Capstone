using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Assets.Resources;
using UnityEngine.SceneManagement;

public class BuffSystem : ComponentSystem
{

    protected override void OnUpdate()
    {
        //ManaRegenBuff
        Entities.ForEach((Entity e, ref PlayerComponent p, ref ManaRegenBuffComp mb) =>
        {
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
        Entities.ForEach((Entity e, ref ProjectileComponent p, ref ProjectileSpeedBuffComp psb, ref MovementComponent m) =>
        {
            if(psb.timer == psb.maxTimer)
            {
                psb.original = m.movementVector;
                m.movementVector = m.movementVector * psb.value;
            }
            
            

            if (psb.timer <= 0)
            {
                m.movementVector = psb.original;
                World.Active.EntityManager.RemoveComponent<ProjectileSpeedBuffComp>(e);
            }
            psb.timer--;

            
            
        });


    }
}
