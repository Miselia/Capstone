using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Events.GenericEvents
{
    public class AddCardtoDeckScrollListEvent : IGenericEvent
    {
        public int cardID;
        public string cardName;

        public AddCardtoDeckScrollListEvent(int id, string name)
        {
            cardID = id;
            cardName = name;
        }
    }
}
