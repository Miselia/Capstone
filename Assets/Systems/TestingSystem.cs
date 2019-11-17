using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class TestingSystem : ComponentSystem
{
    protected override void OnUpdate() {
        Entities.ForEach((ref TestingComponent testingComponent, ref MovementComponent movementComponent) => {
            testingComponent.testingValue += 1f;
            Debug.Log(testingComponent.testingValue);
        });
    }
}
