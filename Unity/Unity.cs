using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

namespace EFO.Unity {

    public class SafeCoroutineBase {
        private const string _SCRIPT_NAME = "[EFOLib.cs] --> SafeCoroutineBase.";
        protected MonoBehaviour _mono;
        protected IEnumerator _corout;

        public SafeCoroutineBase(MonoBehaviour mono)
        {
            _mono = mono;
        }

        public SafeCoroutineBase Stop()
        {
            _mono.StopCoroutine(_corout);
            _corout = null;
            return this;
        }

        ~SafeCoroutineBase()
        {
            if (_corout != null)
                Debug.LogError(_SCRIPT_NAME + "Destructor(): You must call Stop() method in OnDisable Unity Message()");
        }
    }

    public class SafeCoroutine : SafeCoroutineBase {
        private Func<IEnumerator> _func = null;

        public SafeCoroutine(MonoBehaviour mono, Func<IEnumerator> func) : base(mono)
        {
            _func = func;
        }

        public SafeCoroutineBase Start()
        {
            if (base._corout != null) base.Stop();
            _corout = _func();
            base._mono.StartCoroutine(_corout);
            return this;
        }
    }
    public class SafeCoroutine<T1> : SafeCoroutineBase {
        private Func<T1, IEnumerator> _func = null;

        public SafeCoroutine(MonoBehaviour mono, Func<T1, IEnumerator> func) : base(mono)
        {
            _func = func;
        }

        public SafeCoroutineBase Start(T1 firstParam)
        {
            if (base._corout != null) base.Stop();
            _corout = _func(firstParam);
            base._mono.StartCoroutine(_corout);
            return this;
        }
    }
    public class SafeCoroutine<T1, T2> : SafeCoroutineBase {
        private Func<T1, T2, IEnumerator> _func = null;

        public SafeCoroutine(MonoBehaviour mono, Func<T1, T2, IEnumerator> func) : base(mono)
        {
            _func = func;
        }

        public SafeCoroutineBase Start(T1 firstParam, T2 secondParam)
        {
            if (base._corout != null) base.Stop();
            _corout = _func(firstParam, secondParam);
            base._mono.StartCoroutine(_corout);
            return this;
        }
    }

    public class SelfConstSingletonMono<T> : MonoBehaviour where T : MonoBehaviour {
        protected static T singleton;

        protected virtual void Awake()
        {
            CheckSingleton();
        }

        static SelfConstSingletonMono()
        {
            CheckSingleton();

            GameObject go = new GameObject("Handler_" + typeof(T).ToString());
            singleton = go.AddComponent<T>();
        }

        protected static void CheckSingleton()
        {
            int numOfMono = 0;

            foreach (UnityEngine.Object obj in FindObjectsOfType(typeof(T)))
                if (null != (obj as MonoBehaviour)) numOfMono++;

            if (numOfMono > 1)
            {
                string str1 = string.Format("It looks like you have the {0} on a GameObject in your scene. Our prefab-less manager system does not require the {0} " +
                    "to be on a GameObject.\nIt will be added to your scene at runtime automatically for you. Please remove the script from your {1} scene.",
                    typeof(T),
                    SceneManager.GetActiveScene().name);

                Debug.LogError(str1);
            }
        }
    }
}

namespace EFO.Unity.Timer {

    public class SimpleTimer {
        public float StartTime { get; private set; }
        public float EndTime { get; private set; }

        public SimpleTimer() {
            StartTime = -1;
            EndTime = -1;
        }

        public SimpleTimer Start() {
            StartTime = Time.realtimeSinceStartup;
            return this;
        }
        public SimpleTimer Stop() {
            EndTime = Time.realtimeSinceStartup;
            return this;
        }
        public SimpleTimer Resume() {
            EndTime = -1;
            return this;
        }
        public SimpleTimer Reset() {
            StartTime = -1;
            EndTime = -1;
            return this;
        }

        public float GetDuration() {
            if (StartTime == -1)
                throw new Exception("Timer not start please call start() before get duration");

            float ret = -1;

            if (EndTime == -1) ret = Time.realtimeSinceStartup - StartTime;
            else ret = EndTime - StartTime;

            return ret;
        }
    }
}

namespace EFO.Unity.Extensions {

    public static class ActionExtensions {
        private static void Invoke(Delegate listener, object[] args) {
            if (!listener.Method.IsStatic && (listener.Target == null) || (listener.Target as UnityEngine.Object) == null)
                Debug.LogError("an event listener is still subscribed to an event with the method " + listener.Method.Name + " even though it is null. Be sure to balance your event subscriptions.");
            else
                listener.Method.Invoke(listener.Target, args);
        }

        private static void InvokeListners(Delegate[] listeners, object[] args) {
            for (int i = 0; i < listeners.Length; ++i)
                Invoke(listeners[i], args);
        }

        public static void SafeInvoke(this Action action) {
            if (action == null)
                return;

            object[] args = { };
            Delegate[] listeners = action.GetInvocationList();

            InvokeListners(listeners, args);
        }

        public static void SafeInvoke<T1>(this Action<T1> action, T1 arg1) {
            if (action == null)
                return;

            object[] args = { arg1 };
            Delegate[] listeners = action.GetInvocationList();

            InvokeListners(listeners, args);

        }

        public static void SafeInvoke<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2) {
            if (action == null)
                return;

            object[] args = { arg1, arg2 };
            Delegate[] listeners = action.GetInvocationList();

            InvokeListners(listeners, args);
        }

        public static void SafeInvoke<T1, T2, T3>(this Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3) {
            if (action == null)
                return;

            object[] args = { arg1, arg2, arg3 };
            Delegate[] listeners = action.GetInvocationList();

            InvokeListners(listeners, args);
        }
    }
}