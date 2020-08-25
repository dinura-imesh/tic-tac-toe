using UnityEngine;

namespace DefynModules.EventCore.Definitions
{
    //todo try not to clone the event, just maintain two variables instead of copying the whole object
    public class BaseEvent
    {
        public bool done { get; private set; }

        public bool launch => !done;

        private bool markedAndRaisedDone = false;

        protected BaseEvent(E ev)
        {
            done = ev == E.Done;
        }

        public override string ToString()
        {
            return GetType().ToString();
        }

        public BaseEvent MarkAsDone()
        {
            if (markedAndRaisedDone || done)
            {
                Debug.LogError("[InkException] Event is already marked as done");
                return null;
            }

            var eventClone = (BaseEvent) MemberwiseClone();
            eventClone.done = true;
            markedAndRaisedDone = true;
            return eventClone;
        }
    }
}