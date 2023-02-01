using System;

namespace Core.Util {
    [Serializable]
    public class NullableObject {

#if !UNITY_EDITOR
        [NonSerialized]
#endif
        public bool isNull;
    }
}