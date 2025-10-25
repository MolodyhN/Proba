using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using Лаба1._0_GID;

namespace Лаба1._0_GID
{
    public static class LineHelper
    {
        public static Line3D FindLineAtPoint(Point point, List<Line3D> lines, Camera camera, Size viewportSize)
        {
            foreach (var line in lines)
            {
                if (IsPointOn3DLine(point, line, 5, camera, viewportSize))
                    return line;
            }
            return null;
        }

        public static bool IsPointOn3DLine(Point point, Line3D line, float tolerance, Camera camera, Size viewportSize)
        {
            var start2D = ProjectionHelper.Project3DTo2D(line.StartPoint, camera, viewportSize);
            var end2D = ProjectionHelper.Project3DTo2D(line.EndPoint, camera, viewportSize);

            float distance = DistanceToLine(point, start2D, end2D);
            return distance <= tolerance;
        }
        public static float CalculateLineLength3D(Line3D line)
        {
            float dx = line.EndPoint.X - line.StartPoint.X;
            float dy = line.EndPoint.Y - line.StartPoint.Y;
            float dz = line.EndPoint.Z - line.StartPoint.Z;
            return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }
        public static float DistanceBetweenPoints(Point p1, Point p2)
        {
            float dx = p1.X - p2.X;
            float dy = p1.Y - p2.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
        public static float DistanceBetweenPoints(Point3D p1, Point3D p2)
        {
            float dx = p1.X - p2.X;
            float dy = p1.Y - p2.Y;
            float dz = p1.Z - p2.Z;
            return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }
        public static float DistanceBetweenPoints(Point point2D, Point3D point3D, Camera camera, Size viewportSize)
        {
            var projectedPoint = ProjectionHelper.Project3DTo2D(point3D, camera, viewportSize);
            return DistanceBetweenPoints(point2D, projectedPoint);
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

