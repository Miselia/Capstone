﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class DrawSystem : ComponentSystem
{
    private Game game;
    bool[] emptySlots1;
    bool[] emptySlots2;
    protected override void OnStartRunning()
    {
        game = (Game)GameObject.Find("Game").GetComponent(typeof(Game));
        emptySlots1 = new bool[] { false, false, false, false };
        emptySlots2 = new bool[] { false, false, false, false };
    }
    protected override void OnUpdate()
    {
        for(int p =1; p <= 2; p++)
        {
            for (int c = 1; c <= 4; c++)
            {
                bool flag = true;
                Entities.ForEach((ref CardComp card) =>
                {
                    if (card.player == p && card.cardSlot == c) flag = false;
                });
                if (flag)
                {
                    if (game.drawCardFromDeck(p, c) == 0)
                    {
                        setEmpty(p, c);
                        if (checkAllEmpty(p) == true)
                        {
                            game.reshuffle(p);
                            setAllEmpty(p);
                        }
                    }
                  
                }
            }
        }
       



    }
    private void setAllEmpty(int player)
    {
        if (player == 1)
        {
            for (int i = 0; i <= 3; i++)
            {
                emptySlots1[i] = false;
            }
        }
        else
        {
            for (int i = 0; i <= 3; i++)
            {
                emptySlots2[i] = false;
            }
        }
    }
    private bool checkAllEmpty(int player)
    {
        if (player == 1)
        {
            for (int i = 0; i <= 3; i++)
            {
                if (emptySlots1[i] == false) return false;
            }
            return true;
        }
        else
        {
            for (int i = 0; i <= 3; i++)
            {
                if (emptySlots2[i] == false) return false;
            }
            return true;
        }
    }
    private void setEmpty(int player, int card)
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
