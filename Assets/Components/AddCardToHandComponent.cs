using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Entities;

public struct AddCardToHandComponent : IComponentData
{
    public int cardID;
    public int cardSlot;

    public AddCardToHandComponent(int id, int slot)
    {
        cardID = id;
        cardSlot = slot;
    }
}
