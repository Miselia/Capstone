using Unity.Entities;
using UnityEngine;

public class SoundEvent : IGenericEvent
{
    public int sound;

    public SoundEvent(int sound)
    {

        this.sound = sound;
    }
}
