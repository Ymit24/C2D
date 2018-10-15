using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using C2D.Event;

namespace C2D.Event
{
    [RequireComponent(typeof(EventSystem))]
    public class EventComponent : MonoBehaviour
    {
        private EventSystem _localEventSystem;
        protected EventSystem localEventSystem
        {
            get
            {
                if (_localEventSystem == null)
                {
                    _localEventSystem = GetComponent<EventSystem>();
                }
                return _localEventSystem;
            }
        }
    }
}