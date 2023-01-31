using UnityEngine;

namespace Qoqqi.Inspector.Runtime {
    public class SubclassPicker : PropertyAttribute {

        public readonly bool IsRequired;

        public SubclassPicker(bool isRequired) {
            IsRequired = isRequired;
        }
    }
}