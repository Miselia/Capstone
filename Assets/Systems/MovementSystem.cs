using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine.SceneManagement;
using Unity.Jobs;
using Unity.Burst;

public class MovementSystem : JobComponentSystem
{
    /*
    protected override void OnUpdate()
    {
       
            Entities.ForEach((ref MovementComponent moveComp, ref Translation translation) =>
            {
                translation.Value.y += moveComp.movementVector.y * Time.deltaTime;
                translation.Value.x += moveComp.movementVector.x * Time.deltaTime;
            });
        
    }
    */
    [BurstCompile]
    private struct Job: IJobForEachWithEntity<MovementComponent, Translation>
    {
        public float deltaTime;

        public void Execute(Entity entity, int index,ref MovementComponent moveComp, ref Translation translation)
        {
            translation.Value.y += moveComp.movementVector.y * deltaTime;
            translation.Value.x += moveComp.movementVector.x * deltaTime;
        }
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Job job = new Job
        {
            deltaTime = Time.deltaTime,
        };
        return job.Schedule(this, inputDeps);
    }
}
