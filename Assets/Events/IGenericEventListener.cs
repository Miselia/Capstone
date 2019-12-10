using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGenericEventListener
{
    bool HandleEvent(IGenericEvent evt);
}
