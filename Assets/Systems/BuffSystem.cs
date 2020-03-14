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
       

    }
}
