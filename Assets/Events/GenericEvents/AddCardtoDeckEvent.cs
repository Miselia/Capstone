using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Events.GenericEvents
{
    public class AddCardtoDeckEvent : IGenericEvent
    {
        public int cardID;
        public string cardName;
        public AddCardtoDeckEvent(int id, string name)
        {
            cardID = id;
            cardName = name;
        }
    }
}
