using Assets.MonoScript;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace Assets.Systems
{
    public class QuadTreeDrawningSystem : JobComponentSystem
    {
        [BurstCompile]
        private struct Job : IJob
        {
            public float deltaTime;
            public void Execute()
            {
                foreach(QuadTreeNode node in QuadTreeSystem.rootNode.subNodes)
                {

                }
            }
        }

        private void FindDeepestBranchNode(QuadTreeNode)
        {

        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            Job job = new Job
            {
                deltaTime = Time.deltaTime,
            };

            return job.Schedule(inputDeps);
        }
    }
}
