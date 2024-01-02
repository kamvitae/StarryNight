using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace HF_16_StarryNight.Model
{
    class Star
    {
        public Point Location { get; set; }
        public Star(Point location)
        {
            Location = location;
        }
    }
}
