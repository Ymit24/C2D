using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace C2D.Event
{
	public delegate void EventListener(EventInfo ei);
    public class EventSystem : MonoBehaviour
    {
        private static EventSystem _global;
        public static EventSystem Global
        {
            get
            {
                return _global;
            }
        }

		public bool IsGlobal;
        void Awake()
        {
			if (_global == null && IsGlobal)
			{
				_global = this;
			}
			else if (_global != null && IsGlobal)
			{
				Debug.LogWarning("EventSystem: a Global Event System already exists!");
			}
        }

        public Dictionary<Type, List<EventListener>> eventListeners;

		public EventListener RegisterListener<T>(Action<T> listener)
            where T : EventInfo
        {
            Type eventType = typeof(T);
            if (eventListeners == null)
            {
                eventListeners = new Dictionary<Type, List<EventListener>>();
            }
            if (eventListeners.ContainsKey(eventType) == false || eventListeners[eventType] == null)
            {
                eventListeners[eventType] = new List<EventListener>();
            }

            EventListener wrapper = (ei) => { listener( (T) ei ); };

            eventListeners[eventType].Add( wrapper );

			return wrapper;
        }

		public void UnregisterListener<T>(EventListener listener)
		{
			Type eventType = typeof(T);

			if (eventListeners.ContainsKey(eventType) == false) return;
			if (eventListeners[eventType].Contains(listener) == false) return;

			eventListeners[eventType].Remove(listener);
		}

        public void FireEvent(EventInfo eventInfo)
        {
            Type trueEventInfoClass = eventInfo.GetType();
            if (eventListeners == null || eventListeners.ContainsKey(trueEventInfoClass) == false || eventListeners[trueEventInfoClass] == null)
            {
                return;
            }

            foreach (EventListener el in eventListeners[trueEventInfoClass])
            {
                el(eventInfo);
            }
        }
    }
}