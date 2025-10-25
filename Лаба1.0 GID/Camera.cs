using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Лаба1._0_GID
{
    public class Camera
    {
        public Point3D Position { get; set; } = new Point3D(0, 0, 200); // Увеличиваем начальное расстояние
        public Point3D Target { get; set; } = new Point3D(0, 0, 0);
        public Point3D UpVector { get; set; } = new Point3D(0, 1, 0);

        public float Zoom { get; set; } = 1.0f;
        public float RotationX { get; set; } // Вращение вокруг оси X (вертикальное)
        public float RotationY { get; set; } // Вращение вокруг оси Y (горизонтальное)

        public void Reset()
        {
            Position = new Point3D(100, 100, 100);
            Target = new Point3D(0, 0, 0);
            UpVector = new Point3D(0, 1, 0);
            Zoom = 1.0f;
            RotationX = -0.5f;
            RotationY = -0.5f;
        }

        public void Rotate(float deltaX, float deltaY)
        {
            // Горизонтальное вращение (влево-вправо) - вокруг оси Y
            RotationY += deltaX;

            // Вертикальное вращение (вверх-вниз) - вокруг оси X
            RotationX += deltaY;

            // Ограничиваем вертикальное вращение чтобы избежать переворота
            float maxVerticalAngle = (float)(Math.PI / 2.2); // Чуть меньше 90 градусов
            RotationX = Math.Max(-maxVerticalAngle, Math.Min(maxVerticalAngle, RotationX));

            // Нормализуем горизонтальное вращение
            if (RotationY > Math.PI * 2) RotationY -= (float)(Math.PI * 2);
            if (RotationY < -Math.PI * 2) RotationY += (float)(Math.PI * 2);
        }
    }
}

