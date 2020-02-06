using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Assets.Resources;

public class ManaSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
      
        Entities.ForEach(( ref PlayerComponent p) =>
        {
            
            //float newMana = p.mana + p.manaRegen;
            int[] values = { 0, 0, 0 };
            values = p.adjustMana(0);
            

                values = p.setMana(p.maxMana);
            
            EventManager.instance.QueueEvent(new UIUpdateEvent(values[0],values[1],values[2]));
        });
        
    }
}
