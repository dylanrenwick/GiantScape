using System;

namespace GiantScape.Client
{
    public class AsyncPromise
    {
        public event EventHandler Done;

        private bool done;
        public bool IsDone
        {
            get => done;
            set
            {
                done = value;
                if (done) OnDone();
            }
        }

        protected virtual void OnDone()
        {
            Done?.Invoke(this, EventArgs.Empty);
        }
    }

    public class AsyncPromise<T> : AsyncPromise
    {
        public new event EventHandler<T> Done;

        public T Result { get; set; }

        protected override void OnDone()
        {
            Done?.Invoke(this, Result);
        }
    }
}
