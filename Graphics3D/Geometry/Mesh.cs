using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Graphics3D.Geometry
{
    public class Mesh
    {
        public Vertex[] Vertices { get; set; }
        public int[][] Indices { get; set; }

        public Vertex Center
        {
            get
            {
                Vertex center = new Vertex();
                foreach (var v in Vertices)
                {
                    center.X += v.X;
                    center.Y += v.Y;
                    center.Z += v.Z;
                }
                center.X /= Vertices.Length;
                center.Y /= Vertices.Length;
                center.Z /= Vertices.Length;
                return center;
            }
        }

        public Mesh(Tuple<Vertex[], int[][]> data)
            : this(data.Item1, data.Item2)
        {
        }

        public Mesh(Vertex[] vertices, int[][] indices)
        {
            Vertices = vertices;
            Indices = indices;
        }

        public Mesh(string path)
        {
            var vertices = new List<Vertex>();
            var indices = new List<List<int>>();
            var info = File.ReadAllLines(path);
            int index = 0;
            while (info[index].Equals("") || !info[index][0].Equals('v'))
                index++;
            while (info[index].Equals("") || info[index][0].Equals('v'))
            {
                var infoPoint = info[index].Split(' ');
                double x = double.Parse(infoPoint[1]);
                double y = double.Parse(infoPoint[2]);
                double z = double.Parse(infoPoint[3]);
                vertices.Add(new Vertex(x, y, z));
                index++;
            }
            while (info[index].Equals("") || !info[index][0].Equals('f'))
                index++;
            int indexPointSeq = 0;
            while (info[index].Equals("") || info[index][0].Equals('f'))
            {
                var infoPointSeq = info[index].Split(' ');
                var listPoints = new List<int>();
                for (int i = 1; i < infoPointSeq.Length; ++i)
                {
                    int elem;
                    if (int.TryParse(infoPointSeq[i], out elem))
                        listPoints.Add(elem - 1);
                }
                indices.Add(listPoints);
                index++;
                indexPointSeq++;
            }
            Vertices = vertices.ToArray();
            Indices = indices.Select(x => x.ToArray()).ToArray();
        }

        public void Apply(Matrix transformation)
        {
            for (int i = 0; i < Vertices.Length; ++i)
                Vertices[i] *= transformation;
        }

        public void Draw(Graphic3D graphics)
        {
            foreach (var facet in Indices)
            {
                for (int i = 0; i < facet.Length - 1; ++i)
                    graphics.DrawLine(Vertices[facet[i]], Vertices[facet[i + 1]]);
                graphics.DrawLine(Vertices[facet[0]], Vertices[facet[facet.Length - 1]]);
            }
        }

        public void Save(string path)
        {
            string info = "# File Created: " + DateTime.Now.ToString() + "\r\n";
            foreach (var v in Vertices)
                info += "v " + v.X + " " + v.Y + " " + v.Z + "\r\n";
            info += "# " + Vertices.Length + " vertices\r\n";
            foreach (var facet in Indices)
            {
                info += "f ";
                for (int i = 0; i < facet.Length; ++i)
                    info += (facet[i] + 1) + " ";
                info += "\r\n";
            }
            info += "# " + Indices.Length + " polygons\r\n";
            File.WriteAllText(path, info);
        }
    }
}
