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
                    Debug.Log("Gear rotated");
                    float degPerSec = 2;
                    
                    rotation.Value = math.mul(math.normalize(rotation.Value), quaternion.AxisAngle(new float3(0,0,1), degPerSec * deltaTime));
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
