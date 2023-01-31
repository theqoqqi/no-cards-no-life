using System;

namespace Core.Util {
    [Serializable]
    public class NullableObject {

        [NonSerialized]
        public bool isNull;
    }
}