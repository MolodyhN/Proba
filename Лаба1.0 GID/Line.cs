using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Лаба1._0_GID
{
    [Serializable]
    public class Line
    {
        public Point StartPoint { get; set; }
        public Point EndPoint { get; set; }
        public Color Color { get; set; }
        public int Width { get; set; }
        public DashStyle DashStyle { get; set; }
    }
}

