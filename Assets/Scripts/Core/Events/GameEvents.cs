using System;
using System.Collections.Generic;

namespace Core.Events {
    public class GameEvents {

        public static readonly GameEvents Instance = new GameEvents();

        private readonly Dictionary<Type, Action<IGameEvent>> eventListeners =
                new Dictionary<Type, Action<IGameEvent>>();

        public T NewEvent<T>() where T : IGameEvent, new() {
            return new T();
        }

        public void Dispatch(IGameEvent gameEvent) {
            eventListeners[gameEvent.GetType()]?.Invoke(gameEvent);
        }

        public void Dispatch<T>(Action<T> init) where T : IGameEvent, new() {
            var e = NewEvent<T>();
            init(e);
            Dispatch(e);
        }

        public void On<T>(Action<IGameEvent> listener) where T : IGameEvent {
            if (eventListeners.ContainsKey(typeof(T))) {
                eventListeners[typeof(T)] += listener;
            }
            else {
                eventListeners[typeof(T)] = listener;
            }
        }
    }
}