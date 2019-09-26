using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFO.System {
    public class Enum<T> {

        private T _value;
        public Enum(T value) {
            _value = value;
        }

        public static implicit operator T(Enum<T> k) {
            return k._value;
        }

        public T getVal() {
            return _value;
        }
    }
}


