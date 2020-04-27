using Unity.Entities;
using UnityEngine;

public class AnimatorEvent : IGenericEvent
{
    public int pID;
    public string action;

    public AnimatorEvent(int pID, string action)
    {

        this.pID = pID;
        this.action = action;
    }
}
