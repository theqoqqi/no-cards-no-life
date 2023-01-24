using System;
using System.Collections.Generic;

namespace Core.Events {
    public class GameEvents {

        public static readonly GameEvents Instance = new GameEvents();

        private readonly Dictionary<Type, object> eventListeners = new Dictionary<Type, object>();

        public T NewEvent<T>() where T : IGameEvent, new() {
            return new T();
        }

        public void Dispatch<T>(T gameEvent) where T : IGameEvent {
            var eventType = gameEvent.GetType();

            if (!eventListeners.ContainsKey(eventType)) {
                return;
            }
            
            var eventListener = eventListeners[eventType] as Action<T>;
            
            eventListener?.Invoke(gameEvent);
        }

        public void Dispatch<T>(Action<T> init) where T : IGameEvent, new() {
            var e = NewEvent<T>();
            init(e);
            Dispatch(e);
        }

        public void On<T>(Action<T> listener) where T : IGameEvent {
            if (eventListeners.ContainsKey(typeof(T))) {
                eventListeners[typeof(T)] = eventListeners[typeof(T)] as Action<T> + listener;
            }
            else {
                eventListeners[typeof(T)] = listener;
            }
        }

        public void Off<T>(Action<T> listener) where T : IGameEvent {
            if (eventListeners.ContainsKey(typeof(T))) {
                eventListeners[typeof(T)] = eventListeners[typeof(T)] as Action<T> - listener;
            }
        }
    }
}