using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine.SceneManagement;

public class MovementSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        if (SceneManager.GetActiveScene().name.Equals("GameScene") || SceneManager.GetActiveScene().name == "DeckBuilder")
        {
            Entities.ForEach((ref MovementComponent moveComp, ref Translation translation) =>
            {
                translation.Value.y += moveComp.movementVector.y * Time.deltaTime;
                translation.Value.x += moveComp.movementVector.x * Time.deltaTime;
            });
        }
    }
}
