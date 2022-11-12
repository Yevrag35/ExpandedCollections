using System;

namespace MG.Collections.Events
{
    public delegate void ReversedEventHandler(object sender, ReversedEventArgs e);
    public class ReversedEventArgs : EventArgs
    {
        public int Index { get; }
        public int Count { get; }

        public ReversedEventArgs()
            : this(-1, -1)
        {
        }

        public ReversedEventArgs(int index, int count)
            : base()
        {
            this.Index = index;
            this.Count = count;
        }
    }
}
