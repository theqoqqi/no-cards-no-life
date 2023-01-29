using System;
using System.Collections.Generic;

namespace Core.Events {
    public class GameEvents {
        
        /*
         * Посмотреть прошлую версию этого файла можно по этому коммиту: da7e2b5e8ec4a9af6fcca995d1c6dccc757f8879
         * 
         * Если попытаться хранить в eventListeners слушатели с их исходным типом (например, Action<CardUsedEvent>),
         * то придется хранить их в виде object, т.к. указать что-то вроде Action<?>, но такого в C# нет.
         * Поимио этого, если события добавляются в очередь, то необходимый тип события будет доступен только
         * в методе для добавления в очередь (который вызывается из других мест), но до обработки событий в очереди
         * этот тип не дойдет. Там я могу рассчитывать только на IGameEvent.
         * 
         * Я не придумал, как хранить в eventListeners действия с нужным генерик-типом, поэтому решил сделать иначе:
         * 1. Для каждого обработчика, передаваемого в On/Off, я создаю обертку, которая имеет тип Action<IGameEvent>
         * 2. Уже внутри внутри этого обработчика-обертки я кастую событие в нужный тип.
         * 3. В eventListeners хранятся обертки обработчиков, а в listenerMapping лежат пары оригинал -> обертка.
         */

        public static readonly GameEvents Instance = new GameEvents();

        private readonly Dictionary<Type, Action<IGameEvent>> eventListeners =
                new Dictionary<Type, Action<IGameEvent>>();

        private readonly IDictionary<object, Action<IGameEvent>> listenerMapping =
                new Dictionary<object, Action<IGameEvent>>();

        private readonly Queue<IGameEvent> eventQueue = new Queue<IGameEvent>();

        private static T NewEvent<T>() where T : IGameEvent, new() {
            return new T();
        }

        public T Dispatch<T>(Action<T> init) where T : IGameEvent, new() {
            var e = NewEvent<T>();
            
            init(e);
            Enqueue(e);
            
            return e;
        }

        public void DispatchEnqueuedEvents() {
            while (eventQueue.Count > 0) {
                var gameEvent = eventQueue.Dequeue();

                Dispatch(gameEvent);
            }
        }

        private void Enqueue(IGameEvent gameEvent) {
            eventQueue.Enqueue(gameEvent);
        }

        private void Dispatch(IGameEvent gameEvent) {
            var eventType = gameEvent.GetType();

            if (!eventListeners.ContainsKey(eventType)) {
                return;
            }
            
            var eventListener = eventListeners[eventType];
            
            eventListener?.Invoke(gameEvent);
        }

        public void On<T>(Action<T> listener) where T : IGameEvent {
            Action<IGameEvent> mappedListener = e => listener((T) e);
            listenerMapping[listener] = mappedListener;
            
            if (eventListeners.ContainsKey(typeof(T))) {
                eventListeners[typeof(T)] = eventListeners[typeof(T)] + mappedListener;
            }
            else {
                eventListeners[typeof(T)] = mappedListener;
            }
        }

        public void Off<T>(Action<T> listener) where T : IGameEvent {
            var mappedListener = listenerMapping[listener];
            listenerMapping.Remove(mappedListener);
            
            if (eventListeners.ContainsKey(typeof(T))) {
                eventListeners[typeof(T)] = eventListeners[typeof(T)] - mappedListener;
            }
        }
    }
}