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
        public string traits;
        public string flavor;
        public AddCardtoDeckEvent(int id, string name, string t, string f)
        {
            cardID = id;
            cardName = name;
            traits = t;
            flavor = f;
        }
    }
}
