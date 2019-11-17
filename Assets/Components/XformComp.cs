using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct XformComponent : IComponentData
{
    public bool hasXform; // transform flag
    public Vector2 CurrentPosition;
    public Vector2 LastPosition;

    public XformComponent(bool hasXform, Vector2 position)
    {
        this.hasXform = hasXform;
        CurrentPosition = position;
        LastPosition = position;
    }

    public void Move(Vector2 delta)
    {
        MoveHelper(delta.x, delta.y);
    }

    public void MoveHelper(float x, float y)
    {
        LastPosition.x = CurrentPosition.x;
        LastPosition.y = CurrentPosition.y;

        CurrentPosition.x += x;
        CurrentPosition.y += y;
    }
}