using System;
using System.Drawing;

namespace Graphics3D.Geometry
{
    public class Graphic3D
    {
        private static double POINT_SIZE = 5;

        private Graphics graphics;

        public Matrix Transformation { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public Graphic3D(Graphics graphics, Matrix transformation, double width, double height)
        {
            this.graphics = graphics;
            Transformation = transformation;
            Width = width;
            Height = height;
        }

        private Vertex ClipToNormalized(Vertex v)
        {
            return new Vertex(v.X / v.W, v.Y / v.W, v.Z / v.W);
        }

        private PointF NormalizedToScreen(Vertex v)
        {
            return new PointF(
                (float)((v.X + 1) / 2 * Width),
                (float)((-v.Y + 1) / 2 * Height));
        }

        public void DrawLine(Vertex a, Vertex b)
        {
            DrawLine(a, b, Pens.Black);
        }

        public static bool IsHuge(float f)
        {
            return 10e6 < Math.Abs(f);
        }

        public static bool IsHuge(PointF v)
        {
            return IsHuge(v.X) || IsHuge(v.Y);
        }

        public void DrawPoint(Vertex a, Brush brush)
        {
            var t = ClipToNormalized(a * Transformation);
            if (t.Z < -1 || t.Z > 1) return;
            var A = NormalizedToScreen(t);
            if (IsHuge(A)) return;
            var rectangle = new RectangleF(
                (float)(A.X - POINT_SIZE / 2),
                (float)(A.Y - POINT_SIZE / 2),
                (float)POINT_SIZE,
                (float)POINT_SIZE);
            graphics.FillRectangle(brush, rectangle);
        }

        public void DrawLine(Vertex a, Vertex b, Pen pen)
        {
            var t = ClipToNormalized(a * Transformation);
            if (t.Z < -1 || t.Z > 1) return;
            var A = NormalizedToScreen(t);
            var u = ClipToNormalized(b * Transformation);
            if (u.Z < -1 || u.Z > 1) return;
            var B = NormalizedToScreen(u);
            if (IsHuge(A) || IsHuge(B)) return;
            graphics.DrawLine(pen, A, B);
        }
    }
}
