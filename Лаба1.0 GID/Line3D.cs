using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Лаба1._0_GID
{
    [Serializable]
    public class Line3D
    {
        public Point3D StartPoint { get; set; }
        public Point3D EndPoint { get; set; }
        public Color Color { get; set; }
        public int Width { get; set; }
        public DashStyle DashStyle { get; set; }
        public bool IsSelected => SelectionManager.SelectedLine == this;
        public Line3D()
        {
            StartPoint = new Point3D(0, 0, 0);
            EndPoint = new Point3D(0, 0, 0);
        }
    }
}

