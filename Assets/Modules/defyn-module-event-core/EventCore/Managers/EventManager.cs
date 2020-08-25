using System.Collections.Generic;
using DefynModules.EventCore.Definitions;

namespace DefynModules.EventCore.Managers
{
    public class EventManager
    {
        private static EventManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EventManager();
                }

                return instance;
            }
        }

        private static EventManager instance = null;

        public delegate void EventDelegate<T>(T e) where T : BaseEvent;

        private delegate void EventDelegate(BaseEvent e);


        private Dictionary<System.Type, EventDelegate> delegates = new Dictionary<System.Type, EventDelegate>();

        private Dictionary<System.Delegate, EventDelegate> delegateLookup =
            new Dictionary<System.Delegate, EventDelegate>();

        public static void AddListener<T>(EventDelegate<T> del) where T : BaseEvent
        {
            if (Instance.delegateLookup.ContainsKey(del))
            {
                return;
            }

            // Create a new non-generic delegate which calls our generic one.  This
            // is the delegate we actually invoke.
            EventDelegate internalDelegate = (e) => del((T) e);
            Instance.delegateLookup[del] = internalDelegate;

            EventDelegate tempDel;
            if (Instance.delegates.TryGetValue(typeof(T), out tempDel))
            {
                Instance.delegates[typeof(T)] = tempDel += internalDelegate;
            }
            else
            {
                Instance.delegates[typeof(T)] = internalDelegate;
            }
        }


        public static void RemoveListener<T>(EventDelegate<T> del) where T : BaseEvent
        {
            EventDelegate internalDelegate;
            if (Instance.delegateLookup.TryGetValue(del, out internalDelegate))
            {
                EventDelegate tempDel;
                if (Instance.delegates.TryGetValue(typeof(T), out tempDel))
                {
                    tempDel -= internalDelegate;
                    if (tempDel == null)
                    {
                        Instance.delegates.Remove(typeof(T));
                    }
                    else
                    {
                        Instance.delegates[typeof(T)] = tempDel;
                    }
                }

                Instance.delegateLookup.Remove(del);
            }
        }

        // The count of delegate lookups. The delegate lookups will increase by
        // one for each unique AddListener. Useful for debugging and not much else.
        public static int DelegateLookupCount
        {
            get { return Instance.delegateLookup.Count; }
        }

        // Raise the event to all the listeners
        public static void Raise(BaseEvent e)
        {
            EventDelegate del;
            if (Instance.delegates.TryGetValue(e.GetType(), out del))
            {
                del.Invoke(e);
            }
        }
    }
}