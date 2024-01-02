using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace HF_16_StarryNight.Model
{
    class StarChangedEventArgs:EventArgs
    {
        public Star StarThatChanged { get; private set; }
        public bool Removed { get; private set; }

        public StarChangedEventArgs(Star starThatChanged, bool removed)
        {
            StarThatChanged = starThatChanged;
            Removed = removed;
        }
    }
}
