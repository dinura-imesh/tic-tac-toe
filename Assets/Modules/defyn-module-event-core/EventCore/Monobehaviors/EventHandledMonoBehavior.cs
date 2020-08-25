using DefynModules.EventCore.Interfaces;
using UnityEngine;

namespace DefynModules.EventCore.Monobehaviors
{
    public abstract class EventHandledMonoBehavior : MonoBehaviour, IEventHandler
    {
        public abstract void SubscribeEvents();

        public abstract void UnsubscribeEvents();

        protected virtual void OnEnable()
        {
            SubscribeEvents();
        }

        protected virtual void OnDisable()
        {
            UnsubscribeEvents();
        }
    }
}