using Unity.Entities;
using UnityEngine;

public class SoundEvent : IGenericEvent
{
    public string genre;
    public string type;
    public float delay;

    public SoundEvent(string genre, string type)
    {

        this.genre = genre;
        this.type = type;
        this.delay = 0;
    }
    public SoundEvent(string genre, string type, float delay)
    {

        this.genre = genre;
        this.type = type;
        this.delay = delay;
    }
}
