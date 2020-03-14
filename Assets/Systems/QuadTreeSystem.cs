/*
 * Implementation based on Code Monkey tutorial found here: https://www.youtube.com/watch?v=hP4Vu6JbzSo&t=722s
 */

using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Systems
{
    public class QuadTreeSystem : ComponentSystem
    {
        private const int quadrandYMultiplier = 1;
        private const int quadrantCellSize = 5;

        private static int GetPositionHashMapKey(float3 position)
        {
            return (int)(math.floor(position.x / quadrantCellSize) + (quadrandYMultiplier * math.floor(position.y / quadrantCellSize)));
        }

        // Citation: https://answers.unity.com/questions/8338/how-to-draw-a-line-using-script.html
        private void DrawLine(Vector3 origin, Vector3 destination, Color color, float duration = 0.2f)
        {
            GameObject line = new GameObject();
            line.transform.position = origin;
            line.AddComponent<LineRenderer>();
            LineRenderer lr = line.GetComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("Specular"));
            lr.startColor = color;
            lr.endColor = color;
            lr.startWidth = 0.1f;
            lr.endWidth = 0.1f;
            lr.SetPosition(0, origin);
            lr.SetPosition(1, destination);
            GameObject.Destroy(line, duration);
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
            Debug.Log(GetPositionHashMapKey(position) + " " + position);
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

        protected override void OnUpdate()
        {
            EntityQuery eq = GetEntityQuery(typeof(Translation), typeof(CollisionComponent));
            Debug.Log("Entity count = " + eq.CalculateEntityCount());
            NativeMultiHashMap<int, Entity> quadTreeMap = new NativeMultiHashMap<int, Entity>(eq.CalculateEntityCount(), Allocator.TempJob);

            Entities.ForEach((Entity entity, ref Translation translation, ref CollisionComponent col) =>
            {
                int hashMapKey = GetPositionHashMapKey(translation.Value);
                quadTreeMap.Add(hashMapKey, entity);
                //Debug.Log("Entity added to map: Key = " + hashMapKey + " , entity = " + entity.ToString());
            });

            Debug.Log("Total number of entities = " + quadTreeMap.Length);

            DrawQuadrant(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            Debug.Log("Entities in Quadrant " + GetPositionHashMapKey(Input.mousePosition) + " = " + GetEntityCount(quadTreeMap, GetPositionHashMapKey(Input.mousePosition)));
            quadTreeMap.Dispose();
        }
    }
}
