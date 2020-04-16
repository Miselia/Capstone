using UnityEngine;
using Unity.Entities;


public struct GenericBuffComponent : IComponentData
{
    /*
     * Buff Case Values
     * 2 = Mana Regen
     * 3 = Projectile Speed Buff
     * 4 = Curse of the Viper
     */

    public int buffType;
    public float value;
    public float timer;
    public float maxTimer;
    public Vector2 originalMovemementVector;

    public GenericBuffComponent(int type, float val, float time, Vector2 original = new Vector2())
    {
        buffType = type;
        value = val;
        timer = time;
        maxTimer = time;
        originalMovemementVector = original;
    }
}
