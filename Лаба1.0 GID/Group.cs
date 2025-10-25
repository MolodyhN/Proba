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
        public List<Line3D> Lines { get; set; } = new List<Line3D>();
        public Point3D Center { get; private set; } = new Point3D();
        public bool IsSelected => SelectionManager.SelectedGroup == this;

        public void CalculateCenter()
        {
            if (Lines.Count == 0)
            {
                Center = new Point3D(0, 0, 0);
                return;
            }

            float totalX = 0;
            float totalY = 0;
            float totalZ = 0;
            int pointCount = 0;

            foreach (var line in Lines)
            {
                totalX += line.StartPoint.X + line.EndPoint.X;
                totalY += line.StartPoint.Y + line.EndPoint.Y;
                totalZ += line.StartPoint.Z + line.EndPoint.Z;
                pointCount += 2;
            }

            Center = new Point3D(totalX / pointCount, totalY / pointCount, totalZ / pointCount);
        }

        // 3D ПЕРЕМЕЩЕНИЕ
        public void Translate(float dx, float dy, float dz)
        {
            foreach (var line in Lines)
            {
                line.StartPoint = new Point3D(
                    line.StartPoint.X + dx,
                    line.StartPoint.Y + dy,
                    line.StartPoint.Z + dz
                );
                line.EndPoint = new Point3D(
                    line.EndPoint.X + dx,
                    line.EndPoint.Y + dy,
                    line.EndPoint.Z + dz
                );
            }
            CalculateCenter();
        }

        // 3D ВРАЩЕНИЕ ОТНОСИТЕЛЬНО ГЛОБАЛЬНЫХ ОСЕЙ (начала координат)
        public void Rotate3D(float angleX, float angleY, float angleZ)
        {
            float radX = angleX * (float)Math.PI / 180;
            float radY = angleY * (float)Math.PI / 180;
            float radZ = angleZ * (float)Math.PI / 180;

            foreach (var line in Lines)
            {
                line.StartPoint = RotatePointAroundOrigin(line.StartPoint, radX, radY, radZ);
                line.EndPoint = RotatePointAroundOrigin(line.EndPoint, radX, radY, radZ);
            }
            CalculateCenter();
        }

        // 3D ЗЕРКАЛИРОВАНИЕ ОТНОСИТЕЛЬНО ГЛОБАЛЬНЫХ ПЛОСКОСТЕЙ
        public void Mirror3D(bool mirrorX, bool mirrorY, bool mirrorZ)
        {
            foreach (var line in Lines)
            {
                line.StartPoint = MirrorPointAroundAxis(line.StartPoint, mirrorX, mirrorY, mirrorZ);
                line.EndPoint = MirrorPointAroundAxis(line.EndPoint, mirrorX, mirrorY, mirrorZ);
            }
            CalculateCenter();
        }

        // 3D МАСШТАБИРОВАНИЕ ОТНОСИТЕЛЬНО ЦЕНТРА ГРУППЫ
        public void Scale3D(float scaleX, float scaleY, float scaleZ)
        {
            foreach (var line in Lines)
            {
                line.StartPoint = ScalePoint3D(line.StartPoint, Center, scaleX, scaleY, scaleZ);
                line.EndPoint = ScalePoint3D(line.EndPoint, Center, scaleX, scaleY, scaleZ);
            }
            CalculateCenter();
        }

        // ПРАВИЛЬНОЕ ВРАЩЕНИЕ ОТНОСИТЕЛЬНО НАЧАЛА КООРДИНАТ
        private Point3D RotatePointAroundOrigin(Point3D point, float angleX, float angleY, float angleZ)
        {
            float x = point.X;
            float y = point.Y;
            float z = point.Z;

            // Вращение вокруг оси X (меняет Y и Z)
            if (angleX != 0)
            {
                float cosX = (float)Math.Cos(angleX);
                float sinX = (float)Math.Sin(angleX);
                float newY = y * cosX - z * sinX;
                float newZ = y * sinX + z * cosX;
                y = newY;
                z = newZ;
            }

            // Вращение вокруг оси Y (меняет X и Z)
            if (angleY != 0)
            {
                float cosY = (float)Math.Cos(angleY);
                float sinY = (float)Math.Sin(angleY);
                float newX = x * cosY + z * sinY;
                float newZ = -x * sinY + z * cosY;
                x = newX;
                z = newZ;
            }

            // Вращение вокруг оси Z (меняет X и Y)
            if (angleZ != 0)
            {
                float cosZ = (float)Math.Cos(angleZ);
                float sinZ = (float)Math.Sin(angleZ);
                float newX = x * cosZ - y * sinZ;
                float newY = x * sinZ + y * cosZ;
                x = newX;
                y = newY;
            }

            return new Point3D(x, y, z);
        }

        // ЗЕРКАЛИРОВАНИЕ ОТНОСИТЕЛЬНО ГЛОБАЛЬНЫХ ПЛОСКОСТЕЙ
        private Point3D MirrorPointAroundAxis(Point3D point, bool mirrorX, bool mirrorY, bool mirrorZ)
        {
            float x = mirrorX ? -point.X : point.X;
            float y = mirrorY ? -point.Y : point.Y;
            float z = mirrorZ ? -point.Z : point.Z;

            return new Point3D(x, y, z);
        }

        // МАСШТАБИРОВАНИЕ ОТНОСИТЕЛЬНО ЦЕНТРА ГРУППЫ
        private Point3D ScalePoint3D(Point3D point, Point3D center, float scaleX, float scaleY, float scaleZ)
        {
            float dx = point.X - center.X;
            float dy = point.Y - center.Y;
            float dz = point.Z - center.Z;

            return new Point3D(
                center.X + dx * scaleX,
                center.Y + dy * scaleY,
                center.Z + dz * scaleZ
            );
        }

        // Дополнительный метод для перемещения группы в 3D
        public void Move3D(float dx, float dy, float dz)
        {
            Translate(dx, dy, dz);
        }
    }
}