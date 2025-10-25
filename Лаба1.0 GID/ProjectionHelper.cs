using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Лаба1._0_GID
{
    // ProjectionHelper.cs
    public static class ProjectionHelper
    {
        public static Point Project3DTo2D(Point3D point3D, Camera camera, Size viewportSize)
        {
            // Правильная 3D проекция с вращением камеры
            var transformed = TransformPoint(point3D, camera);

            // Перспективная проекция
            float distance = 300.0f;
            float scale = distance / (distance + transformed.Z);

            return new Point(
                (int)(transformed.X * scale * camera.Zoom) + viewportSize.Width / 2,    
                (int)(-transformed.Y * scale * camera.Zoom) + viewportSize.Height / 2
            );
        }

        private static Point3D TransformPoint(Point3D point, Camera camera)
        {
            // Перенос в систему координат камеры
            float x = point.X - camera.Target.X;
            float y = point.Y - camera.Target.Y;
            float z = -(point.Z - camera.Target.Z);

            // Вращение вокруг оси Y (горизонтальное)
            float cosY = (float)Math.Cos(camera.RotationY);
            float sinY = (float)Math.Sin(camera.RotationY);
            float tempZ = z * cosY - x * sinY;  
            float tempX = z * sinY + x * cosY;  
            z = tempZ;
            x = tempX;

            // Вращение вокруг оси X (вертикальное)
            float cosX = (float)Math.Cos(camera.RotationX);
            float sinX = (float)Math.Sin(camera.RotationX);
            float tempY = y * cosX - z * sinX;  
            tempZ = y * sinX + z * cosX;     
            y = tempY;
            z = tempZ;

            return new Point3D(x, y, z);
        }

        public static void Draw3DLine(Graphics g, Point3D start, Point3D end, Color color, int width,
                                    DashStyle style, Camera camera, Size viewportSize)
        {
            var start2D = Project3DTo2D(start, camera, viewportSize);
            var end2D = Project3DTo2D(end, camera, viewportSize);

            using (var pen = new Pen(color, width))
            {
                pen.DashStyle = style;
                g.DrawLine(pen, start2D, end2D);
            }
        }
    }
}
