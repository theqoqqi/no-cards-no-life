using System;
using UnityEngine;

namespace Core.Util {
    [Serializable]
    public class NullableObject {

        [NonSerialized]
        public bool isNull;
    }
}