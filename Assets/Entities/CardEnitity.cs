﻿using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Mathematics;

public static class CardEntity
{

    public static Entity Create(EntityManager em, Vector2 position, int cardID, int cardSlot, int playerID, Mesh mesh, Material mat)
    {
        Entity entity = em.CreateEntity();


        em.AddComponent(entity, typeof(CardComp));
        em.AddComponent(entity, typeof(RenderMesh));
        em.AddComponent(entity, typeof(LocalToWorld));
        em.AddComponent(entity, typeof(Translation));
        em.AddComponent(entity, typeof(Scale));

        em.SetComponentData(entity, new Scale { Value = 1 * 2.35f });
        if(playerID == 1)
        {
            em.SetComponentData(entity, new Translation { Value = new float3(position.x + (cardSlot*2), position.y, 0) });
        }
        if(playerID == 2)
        {
            //Add position change for 2nd player cards
            em.SetComponentData(entity, new Translation { Value = new float3(position.x + (cardSlot * 2), position.y, 0) });
        }
        
        em.SetComponentData(entity, new CardComp(cardID, cardSlot, playerID));
        em.SetSharedComponentData(entity, new RenderMesh { mesh = mesh, material = mat });

        return entity;
    }
}