using System;

namespace Core.GameStates {
    public static class Checks {

        public static T NonNull<T>(T value, string message) {
            if (value == null) {
                throw new NullReferenceException(message);
            }
            
            return value;
        }
    }
}