using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;

namespace Assets.Systems
{
    public class QuadTreeDrawningSystem : JobComponentSystem
    {
        [BurstCompile]
        private struct Job : IJob
        {

            public void Execute()
            {
                throw new NotImplementedException();
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            
        }
    }
}
