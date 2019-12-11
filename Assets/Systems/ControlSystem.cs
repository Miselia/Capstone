using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class ControlSystem : ComponentSystem
{

    protected override void OnUpdate()
    {

        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            spawn(1,1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            spawn(1,2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            spawn(1,3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            spawn(1,4);
        }


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

    private void spawn(int player, int slot)
    {
        //if (spawnListeners != null)
        // {
            int id = 0;
            Entities.ForEach((ref CardComp card) =>
            {
                if (card.cardSlot == slot && card.player == player)
                {
                    id = card.cardID;
                }
            });
            //spawnListeners(cardID, player);
            
        //}
    }
}
