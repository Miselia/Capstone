﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class ControlSystem : ComponentSystem
{
    private float movespeed = 5;
    protected override void OnUpdate()
    {

        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            spawn(1,1);
            Debug.Log("BUTTON DOWN");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            spawn(1,2);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            spawn(1,3);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            spawn(1,4);
        }


        if (Input.GetKeyDown(KeyCode.W))
        {
            Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
            {
                if (player.playerID == 1) move.movementVector.y = movespeed;
            });
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
            {
                if (player.playerID == 1) move.movementVector.x = -movespeed;
            });
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
            {
                if (player.playerID == 1) move.movementVector.y = -movespeed;
            });
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
            {
                if (player.playerID == 1) move.movementVector.x = movespeed;
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
        
        int id = 0;
        Entities.ForEach((ref CardComp card) =>
        {
            if (card.cardSlot == slot && card.player == player)
            {
                id = card.cardID;
            }
        });
        Debug.Log("id of Card: " + id);
        EventManager.instance.QueueEvent(new SpawnEvent(player, id));
        


    }
}
