using Assets.MonoScript;
using Assets.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

public class QuadTreeSystem : JobComponentSystem
{
    public static SortedDictionary<int, QuadTreeNode> quadTreeMap;

    [BurstCompile]
    private struct Job: IJobForEachWithEntity<CollisionComponent, Translation>
    {
        public float deltaTime;

        public static QuadTreeNode rootNode = new QuadTreeNode(new CenteredRectangle(3 * Constants.GameBoundaryOffset, 3 * Constants.GameBoundaryOffset, new Unity.Mathematics.float3()));
        public void Execute(Entity entity, int index, ref CollisionComponent collComp, ref Translation translation)
        {
            rootNode.AddReference(entity, collComp, translation);
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

    protected override void OnCreate()
    {
        quadTreeMap = new SortedDictionary<int, QuadTreeNode>();
        float offset = Constants.GameBoundaryOffset;
        base.OnCreate();
    }
}

