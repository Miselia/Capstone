using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine.SceneManagement;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;

namespace Assets.Systems
{
    public class RotationSystem : JobComponentSystem
    {
        private struct Job : IJobForEachWithEntity<RotationComponent, MovementComponent, Rotation>
        {
            public float deltaTime;
            public void Execute(Entity entity, int index, ref RotationComponent rot, ref MovementComponent mv, ref Rotation rotation)
            {
                if(rot.rotateWithMovementDirection == true)
                {
                    rotation.Value = quaternion.Euler(0, 0, Mathf.Atan2(mv.movementVector.x, mv.movementVector.y));
                }
                else
                {
                    float z = deltaTime * 20;
                    rotation.Value.value.z += z;// = quaternion.Euler(0,0, rotation.Value.value.z + z);
                }
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
}
