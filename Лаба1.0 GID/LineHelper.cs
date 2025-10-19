using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using Лаба1._0_GID;

namespace Лаба1._0_GID
{
    public static class LineHelper
    {
        public static Line FindLineAtPoint(Point point, List<Line> lines)
        {
            foreach (var line in lines)
            {
                if (IsPointOnLine(point, line, 5))
                    return line;
            }
            return null;
        }

        public static bool IsPointOnLine(Point point, Line line, float tolerance)
        {
            float distance = DistanceToLine(point, line.StartPoint, line.EndPoint);
            return distance <= tolerance;
        }
        public static float DistanceBetweenPoints(Point p1, Point p2)
        {
            float dx = p1.X - p2.X;
            float dy = p1.Y - p2.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
        public static float DistanceToLine(Point point, Point lineStart, Point lineEnd)
        {
            float A = point.X - lineStart.X;
            float B = point.Y - lineStart.Y;
            float C = lineEnd.X - lineStart.X;
            float D = lineEnd.Y - lineStart.Y;

            float dot = A * C + B * D;
            float len_sq = C * C + D * D;
            float param = dot / len_sq;

            float xx, yy;

            if (param < 0)
            {
                xx = lineStart.X;
                yy = lineStart.Y;
            }
            else if (param > 1)
            {
                xx = lineEnd.X;
                yy = lineEnd.Y;
            }
            else
            {
                xx = lineStart.X + param * C;
                yy = lineStart.Y + param * D;
            }

            float dx = point.X - xx;
            float dy = point.Y - yy;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public static float CalculateLineLength(Line line)
        {
            float dx = line.EndPoint.X - line.StartPoint.X;
            float dy = line.EndPoint.Y - line.StartPoint.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        public static string GetDashStyleName(DashStyle style)
        {
            switch (style)
            {
                case DashStyle.Solid:
                    return "Сплошная";
                case DashStyle.Dash:
                    return "Пунктир";
                case DashStyle.Dot:
                    return "Точечная";
                case DashStyle.DashDot:
                    return "Штрих-пунктир";
                default:
                    return "Неизвестно";
            }
        }

        public static DashStyle GetDashStyleFromIndex(int index)
        {
            switch (index)
            {
                case 0:
                    return DashStyle.Solid;
                case 1:
                    return DashStyle.Dash;
                case 2:
                    return DashStyle.Dot;
                case 3:
                    return DashStyle.DashDot;
                default:
                    return DashStyle.Solid;
            }
        }

        public static int GetStyleIndex(DashStyle style)
        {
            switch (style)
            {
                case DashStyle.Solid:
                    return 0;
                case DashStyle.Dash:
                    return 1;
                case DashStyle.Dot:
                    return 2;
                case DashStyle.DashDot:
                    return 3;
                default:
                    return 0;
            }
        }
    }
}

