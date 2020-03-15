/*
 * Implementation based on Code Monkey tutorial found here: https://www.youtube.com/watch?v=hP4Vu6JbzSo&t=722s
 */

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Systems
{
    public struct QuadrantData
    {
        public Entity entity;
        public float3 position;
    }
    public class QuadTreeSystem : ComponentSystem
    {
        private const int quadrandYMultiplier = 10;
        private const int quadrantCellSize = 5;

        private static int GetPositionHashMapKey(float3 position)
        {
            return (int)(math.floor(position.x / quadrantCellSize) + (quadrandYMultiplier * math.floor(position.y / quadrantCellSize)));
        }

        private void DrawQuadrant(float3 position)
        {
            Vector3 lowerLeft = new Vector3(
                math.floor(position.x / quadrantCellSize) * quadrantCellSize,
                math.floor(position.y / quadrantCellSize) * quadrantCellSize);

            /*DrawLine(lowerLeft, lowerLeft + new Vector3(1, 0) * quadrantCellSize, Color.white);
            DrawLine(lowerLeft, lowerLeft + new Vector3(0, 1) * quadrantCellSize, Color.white);
            DrawLine(lowerLeft + new Vector3(1, 0) * quadrantCellSize, lowerLeft, Color.white);
            DrawLine(lowerLeft + new Vector3(0, 1) * quadrantCellSize, lowerLeft, Color.white);*/
            Debug.DrawLine(lowerLeft, lowerLeft + new Vector3(1, 0) * quadrantCellSize);
            Debug.DrawLine(lowerLeft, lowerLeft + new Vector3(0, 1) * quadrantCellSize);
            Debug.DrawLine(lowerLeft + new Vector3(1, 0) * quadrantCellSize, lowerLeft + new Vector3(1,1) * quadrantCellSize);
            Debug.DrawLine(lowerLeft + new Vector3(0, 1) * quadrantCellSize, lowerLeft + new Vector3(1,1) * quadrantCellSize);
            //Debug.Log(GetPositionHashMapKey(position) + " " + position);
        }

        private int GetEntityCount(NativeMultiHashMap<int, Entity> map, int key)
        {
            Entity entity;
            NativeMultiHashMapIterator<int> itr;
            int count = 0;
            if(map.TryGetFirstValue(key, out entity, out itr))
            {
                do
                {
                    count++;
                }
                while (map.TryGetNextValue(out entity, ref itr));
            }
            return count;
        }

        [BurstCompile]
        private struct SetQuadrantHashMapJob : IJobForEachWithEntity<Translation, CollisionComponent>
        {
            public NativeMultiHashMap<int, QuadrantData>.ParallelWriter quadTreeMap;

            public void Execute(Entity entity, int index, ref Translation translation, ref CollisionComponent col)
            {
                int hashMapKey = GetPositionHashMapKey(translation.Value);
                quadTreeMap.Add(hashMapKey, new QuadrantData {
                    entity = entity,
                    position = translation.Value
                });
            }
        }

        protected override void OnUpdate()
        {
            EntityQuery eq = GetEntityQuery(typeof(Translation), typeof(CollisionComponent));
            Debug.Log("Entity count = " + eq.CalculateEntityCount());
            NativeMultiHashMap<int, QuadrantData> quadTreeMap = new NativeMultiHashMap<int, QuadrantData>(eq.CalculateEntityCount(), Allocator.TempJob);

            SetQuadrantHashMapJob sqj = new SetQuadrantHashMapJob
            {
                quadTreeMap = quadTreeMap.AsParallelWriter(),
            };
            JobHandle job = JobForEachExtensions.Schedule(sqj, eq);
            job.Complete();

            //Debug.Log("Total number of entities = " + quadTreeMap.Length);

            DrawQuadrant(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            //Debug.Log("Entities in Quadrant " + GetPositionHashMapKey(Camera.main.ScreenToWorldPoint(Input.mousePosition)) + " = " + GetEntityCount(quadTreeMap, GetPositionHashMapKey(Camera.main.WorldToScreenPoint(Input.mousePosition))));
            quadTreeMap.Dispose();
        }
    }
}
