﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using UnityEngine.SceneManagement;

public class DrawSystem : ComponentSystem
{
    private Game game;
    bool[] emptySlots1;
    bool[] emptySlots2;
    bool gameInitialized;

    private void Initialize()
    {
        /*
        game = (Game)GameObject.Find("Game").GetComponent(typeof(Game));
        emptySlots1 = new bool[] { false, false, false, false };
        emptySlots2 = new bool[] { false, false, false, false };
        gameInitialized = true;
        */
    }

    protected override void OnUpdate()
    {
        

        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            game = (Game)GameObject.Find("Game").GetComponent(typeof(Game));

            emptySlots1 = new bool[] { false, false, false, false };
            emptySlots2 = new bool[] { false, false, false, false };
            Entities.ForEach((ref CardComp card) =>
            {
                if (card.player == 1 && card.cardSlot == 1) emptySlots1[0] = true;
                if (card.player == 1 && card.cardSlot == 2) emptySlots1[1] = true;
                if (card.player == 1 && card.cardSlot == 3) emptySlots1[2] = true;
                if (card.player == 1 && card.cardSlot == 4) emptySlots1[3] = true;
                if (card.player == 2 && card.cardSlot == 1) emptySlots2[0] = true;
                if (card.player == 2 && card.cardSlot == 2) emptySlots2[1] = true;
                if (card.player == 2 && card.cardSlot == 3) emptySlots2[2] = true;
                if (card.player == 2 && card.cardSlot == 4) emptySlots2[3] = true;
            });
            for (int p = 1; p <= 2; p++)
            {
                bool[] hand;
                if (p == 1) hand = emptySlots1;
                else hand = emptySlots2;
                for (int c = 1; c <= 4; c++)
                {
                    if (hand[c-1] == false)
                    {
                        if (game.DrawCardFromDeck(p, c) == 0)
                        {
                            if (CheckAllEmpty(p) == true)
                            {
                                game.Reshuffle(p);
                            }
                        }
                    }
                    
                }
            }
            
        }
    }

    private bool CheckAllEmpty(int player)
    {
        if (player == 1)
        {
            for (int i = 0; i <= 3; i++)
            {
                if (emptySlots1[i] == true) return false;
            }
            return true;
        }
        else
        {
            for (int i = 0; i <= 3; i++)
            {
                if (emptySlots2[i] == true) return false;
            }
            return true;
        }
    }
    private void SetEmpty(int player, int card)
    {
        if (player == 1)
        {
             emptySlots1[card - 1] = true;
        }
        else
        {
            emptySlots2[card - 1] = true;
        }
    }
}
