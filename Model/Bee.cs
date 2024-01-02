using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace HF_16_StarryNight.Model
{
    class Bee
    {
        public Point Location { get; set; }
        public Size Size { get; set; }
        public Rect Position { get => new Rect(Location, Size); }
        public double Width { get => Position.Width; }
        public double Height { get => Position.Height; }

        public Bee(Point location, Size size)
        {
            Location = location;
            Size = size;
        }
    }
}
