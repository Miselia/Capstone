using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class ControlSystem : ComponentSystem
{

    //Spawner spawner = (Game)FindObjectOfType(typeof(Game)).getSpawner();
    protected override void OnUpdate()
    {

        /*
        if (Input.GetKeyDown("Q"))
        {
            
            Entities.ForEach((ref CardComp card,ref ) =>
            {
                
            });
        }
        */
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
            {
                if (player.playerID == 1) move.movementVector.y = 5;
            });
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
            {
                if (player.playerID == 1) move.movementVector.x = -5;
            });
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
            {
                if (player.playerID == 1) move.movementVector.y = -5;
            });
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
            {
                if (player.playerID == 1) move.movementVector.x = 5;
            });
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
            {
                if (player.playerID == 1) move.movementVector.y = 0;
            });
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
            {
                if (player.playerID == 1) move.movementVector.x = 0;
            });
        }



    }
}
