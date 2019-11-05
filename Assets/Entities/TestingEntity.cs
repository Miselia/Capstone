using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class TestingEntity : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        EntityManager entityManager = World.Active.EntityManager;
        Entity testEntity = entityManager.CreateEntity(typeof(TestingComponent));

        entityManager.SetComponentData(testEntity, new TestingComponent { testingValue = 10 });
    }
}
