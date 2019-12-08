using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public interface IGenericEventListener
    {
        bool HandleEvent(IGenericEvent evt);
    }
}
