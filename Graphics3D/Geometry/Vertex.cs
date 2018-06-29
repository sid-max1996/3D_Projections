using System;

namespace Graphics3D.Geometry
{
    public struct Vertex
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double W { get; set; }

        public Vertex(double x, double y, double z, double w = 1)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public static Vertex operator *(double x, Vertex v)
        {
            for (int i = 0; i < 4; ++i)
                v[i] *= x;
            return v;
        }

        public static Vertex operator *(Vertex v, double x)
        {
            return x * v;
        }

        public static Vertex operator /(Vertex v, double x)
        {
            return v * (1 / x);
        }

        public static Vertex operator +(double x, Vertex v)
        {
            return v + x;
        }

        public static Vertex operator +(Vertex v, double x)
        {
            for (int i = 0; i < 4; ++i)
                v[i] += x;
            return v;
        }

        public static Vertex operator -(Vertex v, double x)
        {
            return v + (-x);
        }

        public static Vertex operator -(double x, Vertex v)
        {
            return v + (-x);
        }

        public static Vertex operator -(Vertex v)
        {
            return -1 * v;
        }

        // Скалярное произведение векторов
        public static double DotProduct(Vertex u, Vertex v)
        {
            double result = 0;
            for (int i = 0; i < 3; ++i)
                result += u[i] * v[i];
            return result;
        }

        // Векторное произведение векторов
        public static Vertex CrossProduct(Vertex u, Vertex v)
        {
            return new Vertex(
                u[1] * v[2] - u[2] * v[1],
                u[2] * v[0] - u[0] * v[2],
                u[0] * v[1] - u[1] * v[0]);
        }

        public static Vertex operator *(Vertex v, Matrix m)
        {
            var result = v;
            for (int i = 0; i < 4; ++i)
            {
                result[i] = 0;
                for (int j = 0; j < 4; ++j)
                    result[i] += v[j] * m[j, i];
            }
            return result;
        }

        public double this[int i]
        {
            get
            {
                switch (i)
                {
                    case 0: return X;
                    case 1: return Y;
                    case 2: return Z;
                    case 3: return W;
                    default: throw new IndexOutOfRangeException("Vertex has only 4 coordinates");
                }
            }
            set
            {
                switch (i)
                {
                    case 0: X = value; break;
                    case 1: Y = value; break;
                    case 2: Z = value; break;
                    case 3: W = value; break;
                    default: throw new IndexOutOfRangeException("Vertex has only 4 coordinates");
                }
            }
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", X, Y, Z);
        }
    }
}
