using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CreateProjectileEvent : IGenericEvent
{
    public string type;
    public int damage;
    public Vector2 position;
    public Vector2 movementVector;
    public float radius;
    public int timer;
    public int matIndex;

    public CreateProjectileEvent(string t, int d, Vector2 pos, Vector2 mov, float rad, int time)
    {
        type = t;
        damage = d;
        position = pos;
        movementVector = mov;
        radius = rad;
        timer = time;
    }
}
