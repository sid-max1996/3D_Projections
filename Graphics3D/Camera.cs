using Graphics3D.Geometry;
using System;

namespace Graphics3D
{
    class Camera
    {
        public Vertex Position { get; set; }
        public double AngleY { get; set; }
        public double AngleX { get; set; }
        public Matrix Projection { get; set; }

        public Vertex Forward { get { return new Vertex(-Math.Sin(AngleY), Math.Sin(AngleX), -Math.Cos(AngleY)); } }
        public Vertex Left { get { return new Vertex(-Math.Sin(AngleY + Math.PI / 2), 0, -Math.Cos(AngleY + Math.PI / 2)); } }
        public Vertex Up { get { return Vertex.CrossProduct(Forward, Left); } }
        public Vertex Right { get { return -Left; } }
        public Vertex Backward { get { return -Forward; } }
        public Vertex Down { get { return -Up; } }

        public Matrix ViewProjection
        {
            get
            {
                return Transformations.Translate(-Position)
                    * Transformations.RotateY(-AngleY)
                    * Transformations.RotateX(-AngleX)
                    * Projection;
            }
        }

        public Camera(Vertex position, double angleY, double angleX, Matrix projection)
        {
            Position = position;
            AngleY = angleY;
            AngleX = angleX;
            Projection = projection;
        }
    }
}
