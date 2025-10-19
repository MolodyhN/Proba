using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Лаба1._0_GID
{
    [Serializable]
    public class Group
    {
        public string Name { get; set; }
        public List<Line> Lines { get; set; } = new List<Line>();
        public Point Center { get; private set; }

        public void CalculateCenter()
        {
            if (Lines.Count == 0)
            {
                Center = Point.Empty;
                return;
            }

            int totalX = 0;
            int totalY = 0;
            int pointCount = 0;

            foreach (var line in Lines)
            {
                totalX += line.StartPoint.X + line.EndPoint.X;
                totalY += line.StartPoint.Y + line.EndPoint.Y;
                pointCount += 2;
            }

            Center = new Point(totalX / pointCount, totalY / pointCount);
        }

        public void Move(int dx, int dy)
        {
            foreach (var line in Lines)
            {
                line.StartPoint = new Point(line.StartPoint.X + dx, line.StartPoint.Y + dy);
                line.EndPoint = new Point(line.EndPoint.X + dx, line.EndPoint.Y + dy);
            }
            CalculateCenter();
        }

        public void Rotate(float angle)
        {
            foreach (var line in Lines)
            {
                line.StartPoint = RotatePoint(line.StartPoint, Center, angle);
                line.EndPoint = RotatePoint(line.EndPoint, Center, angle);
            }
        }

        public void Scale(float scaleX, float scaleY)
        {
            foreach (var line in Lines)
            {
                line.StartPoint = ScalePoint(line.StartPoint, Center, scaleX, scaleY);
                line.EndPoint = ScalePoint(line.EndPoint, Center, scaleX, scaleY);
            }
            CalculateCenter();
        }

        public void Mirror(bool horizontal)
        {
            foreach (var line in Lines)
            {
                line.StartPoint = MirrorPoint(line.StartPoint, Center, horizontal);
                line.EndPoint = MirrorPoint(line.EndPoint, Center, horizontal);
            }
        }

        private Point RotatePoint(Point point, Point center, float angle)
        {
            double radians = angle * System.Math.PI / 180;
            double cos = System.Math.Cos(radians);
            double sin = System.Math.Sin(radians);

            int dx = point.X - center.X;
            int dy = point.Y - center.Y;

            int newX = (int)(dx * cos - dy * sin) + center.X;
            int newY = (int)(dx * sin + dy * cos) + center.Y;

            return new Point(newX, newY);
        }

        private Point ScalePoint(Point point, Point center, float scaleX, float scaleY)
        {
            int dx = point.X - center.X;
            int dy = point.Y - center.Y;

            int newX = (int)(dx * scaleX) + center.X;
            int newY = (int)(dy * scaleY) + center.Y;

            return new Point(newX, newY);
        }

        private Point MirrorPoint(Point point, Point center, bool horizontal)
        {
            if (horizontal)
            {
                return new Point(2 * center.X - point.X, point.Y);
            }
            else
            {
                return new Point(point.X, 2 * center.Y - point.Y);
            }
        }
    }
}