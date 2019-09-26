using System;
using UnityEngine;

namespace EFO.Unity.AsyncOperation {

    public abstract class AsyncOperation : CustomYieldInstruction {

        public abstract bool GetDone();

        public virtual float GetProgress()
        {
            return -1;
        }

        public sealed override bool keepWaiting {
            get { return !GetDone(); }
        }
    }

    public abstract class AsyncRequest<TData> : AsyncOperation {
        protected bool isDone = false;
        protected TData data;
        private bool _isFirstSent = true;

        public TData GetData() {
            if (!isDone) throw new Exception("Don't read data while operation not done");
            return data;
        }

        public sealed override bool GetDone() {
            return isDone;
        }

        public AsyncOperation Send() {
            if (!isDone && !_isFirstSent) throw new Exception("Don't call send method while operation not done");
            _isFirstSent = false;
            isDone = false;
            SendOperation();
            return this;
        }

        protected abstract void SendOperation();
    }

}
