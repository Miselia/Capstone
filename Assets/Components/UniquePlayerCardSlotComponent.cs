using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

public struct UniquePlayerCardSlotComponent : IComponentData
{
    public int playerID;
    public int cardSlot;

    public UniquePlayerCardSlotComponent(int id, int slot)
    {
        playerID = id;
        cardSlot = slot;
    }
}
