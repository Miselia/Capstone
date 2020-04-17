using Unity.Entities;
using UnityEngine;

public class SoundEvent : IGenericEvent
{
    public string genre;
    public string type;

    public SoundEvent(string genre, string type)
    {

        this.genre = genre;
        this.type = type;
    }
}
