using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics3D.Geometry
{
    public class RotationFigure : Mesh
    {
        public RotationFigure(IList<Vertex> initial, int axis, int density)
            : base(Construct(initial, axis, density))
        {
        }

        private static Tuple<Vertex[], int[][]> Construct(IList<Vertex> initial, int axis, int density)
        {
            Debug.Assert(0 <= axis && axis < 3);
            var n = initial.Count;
            var vertices = new Vertex[n * density];
            var indices = new int[density * (n - 1)][];
            Func<double, Matrix> rotation;
            switch (axis)
            {
                case 0: rotation = Transformations.RotateX; break;
                case 1: rotation = Transformations.RotateY; break;
                default: rotation = Transformations.RotateZ; break;
            }
            for (int i = 0; i < density; ++i)
                for (int j = 0; j < n; ++j)
                    vertices[i * n + j] = initial[j] * rotation(2 * Math.PI * i / density);
            for (int i = 0; i < density; ++i)
                for (int j = 0; j < n - 1; ++j)
                    indices[i * (n - 1) + j] = new int[4] {
                        i * n + j,
                        (i + 1) % density * n + j,
                        (i + 1) % density * n + j + 1,
                        i * n + j + 1 };
            return new Tuple<Vertex[], int[][]>(vertices, indices);
        }
    }
}
