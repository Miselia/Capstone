using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine.SceneManagement;
using Assets.Resources;

public class ControlSystem : ComponentSystem
{
    private float movespeed = Constants.PlayerMovementSpeed;
    protected override void OnUpdate()
    {
       
            //Player 1
            if (Input.GetKeyDown(KeyCode.Q))
            {
                spawn(1, 1);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                spawn(1, 2);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                spawn(1, 3);
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                spawn(1, 4);
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                if (Input.GetKey(KeyCode.S))
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 1) move.movementVector.y = 0;
                    });
                }
                else
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 1) move.movementVector.y = movespeed;
                    });
                }
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                if (Input.GetKey(KeyCode.W))
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 1) move.movementVector.y = 0;
                    });
                }
                else
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 1) move.movementVector.y = -movespeed;
                    });
                }
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                if (Input.GetKey(KeyCode.D))
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 1) move.movementVector.x = 0;
                    });
                }
                else
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 1) move.movementVector.x = -movespeed;
                    });
                }
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 1) move.movementVector.x = 0;
                    });
                }
                else
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 1) move.movementVector.x = movespeed;
                    });
                }
            }

            if (Input.GetKeyUp(KeyCode.W))
            {
                if (Input.GetKey(KeyCode.S))
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 1) move.movementVector.y = -movespeed;
                    });
                }
                else
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 1) move.movementVector.y = 0;
                    });
                }
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                if (Input.GetKey(KeyCode.W))
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 1) move.movementVector.y = movespeed;
                    });
                }
                else
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 1) move.movementVector.y = 0;
                    });
                }
            }

            if (Input.GetKeyUp(KeyCode.A))
            {
                if (Input.GetKey(KeyCode.D))
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 1) move.movementVector.x = movespeed;
                    });
                }
                else
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 1) move.movementVector.x = 0;
                    });
                }
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                if (Input.GetKey(KeyCode.A))
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 1) move.movementVector.x = -movespeed;
                    });
                }
                else
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 1) move.movementVector.x = 0;
                    });
                }
            }

            //PLAYER 2
            if (Input.GetKeyDown(KeyCode.Y))
            {
                spawn(2, 1);
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                spawn(2, 2);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                spawn(2, 3);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                spawn(2, 4);
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                if (Input.GetKey(KeyCode.K))
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 2) move.movementVector.y = 0;
                    });
                }
                else
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 2) move.movementVector.y = movespeed;
                    });
                }
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                if (Input.GetKey(KeyCode.I))
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 2) move.movementVector.y = 0;
                    });
                }
                else
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 2) move.movementVector.y = -movespeed;
                    });
                }
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                if (Input.GetKey(KeyCode.L))
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 2) move.movementVector.x = 0;
                    });
                }
                else
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 2) move.movementVector.x = -movespeed;
                    });
                }
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                if (Input.GetKey(KeyCode.J))
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 2) move.movementVector.x = 0;
                    });
                }
                else
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 2) move.movementVector.x = movespeed;
                    });
                }
            }

            if (Input.GetKeyUp(KeyCode.I))
            {
                if (Input.GetKey(KeyCode.K))
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 2) move.movementVector.y = -movespeed;
                    });
                }
                else
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 2) move.movementVector.y = 0;
                    });
                }
            }
            if (Input.GetKeyUp(KeyCode.K))
            {
                if (Input.GetKey(KeyCode.I))
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 2) move.movementVector.y = movespeed;
                    });
                }
                else
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 2) move.movementVector.y = 0;
                    });
                }
            }

            if (Input.GetKeyUp(KeyCode.J))
            {
                if (Input.GetKey(KeyCode.L))
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 2) move.movementVector.x = movespeed;
                    });
                }
                else
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 2) move.movementVector.x = 0;
                    });
                }
            }
            if (Input.GetKeyUp(KeyCode.L))
            {
                if (Input.GetKey(KeyCode.J))
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 2) move.movementVector.x = -movespeed;
                    });
                }
                else
                {
                    Entities.ForEach((ref PlayerComponent player, ref MovementComponent move) =>
                    {
                        if (player.playerID == 2) move.movementVector.x = 0;
                    });
                }
            }
        
    }

    private void spawn(int player, int slot)
    {
        
        int id = 0;
        Entity p = new Entity();

        Entities.ForEach((Entity e, ref PlayerComponent pID) =>
        {
            if (pID.playerID == player) p = e;
        });

        Entities.ForEach((Entity e, ref CardComp card) =>
        {
            if (card.cardSlot == slot && card.player == player)
            {
                Debug.Log("Card Slot: " + card.cardSlot + " Player: " + card.player);
                id = card.cardID;
                EventManager.instance.QueueEvent(new SpawnEvent(e, p));
                EventManager.instance.QueueEvent(new DeckBuilderHandAdjustEvent(e));
            }
        });
        
            
    }
}
