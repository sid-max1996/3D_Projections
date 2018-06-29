using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics3D.Geometry
{
    public class Plot : Mesh
    {
        //f(x, y) = (x^2*y)/(x^4+y^2) / 2
        private static double F(double x, double y)
        {
            return (x * x * y) / (x * x * x * x + y * y) / 2;
        }

        //x0 = y0 = 0.8 x1 = y1 = 0.8 
        public Plot()
            : this(-0.8, 0.8, 0.05, -0.8, 0.8, 0.05, F)
        {
        }

        public Plot(double x0, double x1, double dx,
                double y0, double y1, double yz,
                Func<double, double, double> function)
            : base(Construct(x0, x1, dx, y0, y1, yz, function))
        {
        }

        private static Tuple<Vertex[], int[][]> Construct(
            double x0, double x1, double dx, double y0, double y1, double dy,
            Func<double, double, double> function)
        {
            int nx = (int)((x1 - x0) / dx);
            int nz = (int)((y1 - y0) / dy);
            var vertices = new Vertex[nx * nz];
            var indices = new int[(nx - 1) * (nz - 1)][];
            for (int i = 0; i < nx; ++i)
                for (int j = 0; j < nz; ++j)
                {
                    var x = x0 + dx * i;
                    var y = y0 + dy * j;
                    vertices[i * nz + j] = new Vertex(x, function(x, y), y);
                }
            for (int i = 0; i < nx - 1; ++i)
                for (int j = 0; j < nz - 1; ++j)
                {
                    indices[i * (nz - 1) + j] = new int[4] {
                        i * nz + j,
                        (i + 1) * nz + j,
                        (i + 1) * nz + j + 1,
                        i * nz + j + 1 };
                }
            return new Tuple<Vertex[], int[][]>(vertices, indices);
        }
    }
}
