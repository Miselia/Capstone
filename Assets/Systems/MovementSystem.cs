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
    [BurstCompile]
    private struct Job: IJobForEachWithEntity<MovementComponent, Translation>
    {
        public float deltaTime;

        public void Execute(Entity entity, int index,ref MovementComponent moveComp, ref Translation translation)
        {
            translation.Value.y += moveComp.movementVector.y * deltaTime;
            translation.Value.x += moveComp.movementVector.x * deltaTime;

            // This works, but needs to only apply to player entities
            /*if(translation.Value.y > 5.7f)
            {
                translation.Value.y = 5.7f;
            }
            else if (translation.Value.y < -5.7f)
            {
                translation.Value.y = -5.7f;
            }*/
            // This needs to be adjusted based on scene and player number
            /*else if (translation.Value.x > 5.7f)
            {
                translation.Value.x = 5.7f;
            }
            else if (translation.Value.x < -5.7f)
            {
                translation.Value.x = -5.7f;
            }*/
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
