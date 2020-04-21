using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System;
using Assets.Resources;

namespace Assets.MonoScript
{
    interface IGame : IGenericEventListener
    {
        bool HandleEvent(IGenericEvent evt);
        Dictionary<Entity, List<Entity>> GetCollidingPairs();

        int AddCardToHandFromCardLibrary(int player, int cardSlot, int cardID);
    }
}
