using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Assets.Resources;
using UnityEngine.SceneManagement;

public class PlayerValueSystem : ComponentSystem
{
    
    protected override void OnUpdate()
    {
        
            /*Entities.ForEach((Entity e, ref PlayerComponent p, ref ManaDeltaComp delta) =>
            {

                p.mana = p.mana + delta.delta;
                World.Active.EntityManager.RemoveComponent<ManaDeltaComp>(e);

            });
            Entities.ForEach((Entity e, ref PlayerComponent p, ref HealthDeltaComp delta) =>
            {
                if(delta.delta == -1) EventManager.instance.QueueEvent(new SoundEvent(p.GetGenre(), "Hurt")); ;
                if (delta.delta < -1) EventManager.instance.QueueEvent(new SoundEvent(p.GetGenre(), "HurtHeavy")); ;
                if(delta.delta < 0) EventManager.instance.QueueEvent(new AnimatorEvent(p.playerID, "Hurt"));
                if(delta.delta >0) EventManager.instance.QueueEvent(new SoundEvent(p.GetGenre(), "BuffSelf")); ;
                p.healthRemaining = p.healthRemaining + delta.delta;
                World.Active.EntityManager.RemoveComponent<HealthDeltaComp>(e);
                

            });*/
            Entities.ForEach((Entity e, ref PlayerComponent p) =>
            {
                if(EntityManager.HasComponent(e, typeof(ManaDeltaComp)))
                {
                    p.mana = p.mana + EntityManager.GetComponentData<ManaDeltaComp>(e).delta;
                    EntityManager.RemoveComponent<ManaDeltaComp>(e);
                }
                if(EntityManager.HasComponent(e, typeof(HealthDeltaComp)))
                {
                    int delta = EntityManager.GetComponentData<HealthDeltaComp>(e).delta;
                    if (delta < 0)
                        EventManager.instance.QueueEvent(new SoundEvent("Fantasy","Hurt"));
                    p.healthRemaining = p.healthRemaining + delta;
                    EntityManager.RemoveComponent<HealthDeltaComp>(e);
                }

                p.mana = p.mana + p.manaRegen;

                if (p.mana > p.maxMana)
                {
                    p.mana = p.maxMana;
                }
                if (p.healthRemaining <= 0) {
                    //EventManager.instance.QueueEvent(new SoundEvent("Other", "Destroy"));
                    EventManager.instance.QueueEvent(new GameOverEvent(p.playerID));
                    Debug.Log("Game Over Event Sent");
                    p.healthRemaining = Constants.PlayerMaximumHealth;
                    
                }
                if (p.healthRemaining > Constants.PlayerMaximumHealth)
                {
                    p.healthRemaining = Constants.PlayerMaximumHealth;
                }
                
                EventManager.instance.QueueEvent(new UIUpdateEvent(p.healthRemaining, (int)Mathf.Floor(p.mana), p.playerID));

            });
        
    }
}
