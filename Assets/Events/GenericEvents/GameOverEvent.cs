using Unity.Entities;
using UnityEngine;

public class GameOverEvent : IGenericEvent
{
    public int pID;

    public GameOverEvent(int pID)
    {

        this.pID = pID;
    }
}
