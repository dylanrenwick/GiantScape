using System;

namespace GiantScape.Client
{
    [Serializable]
    public class EventState<T>
    {
        public T State { get; set; }
        public bool Handled { get; set; }

        public EventState(T state)
        {
            State = state;
        }
    }
}
