using System;
using System.Collections.Generic;
using System.Text;

namespace HF_16_StarryNight.Model
{
    class BeeMovedEventArgs : EventArgs
    {
        public Bee BeeThatMoved { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }

        public BeeMovedEventArgs(Bee beeThatMoved, double x, double y)
        {
            BeeThatMoved = beeThatMoved;
            X = x;
            Y = y;
        }
    }
}
