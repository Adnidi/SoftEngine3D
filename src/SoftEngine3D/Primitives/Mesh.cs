using System;
using System.Collections.Generic;
using System.Drawing;

namespace SoftEngine3D.Primitives
{
    public class Mesh
    {
        public Mesh(List<Vertex> vertices, List<Face> faces)
        {
            Vertices = vertices;
            Faces = faces;
            Position = new Vector3();
            Rotation = new Vector3();
        }

        public Mesh(List<Vertex> vertices) : this(vertices, new List<Face>())
        {
        }

        public Mesh() : this(new List<Vertex>(), new List<Face>())
        {
        }

        public List<Vertex> Vertices { get; private set; }
        public List<Face> Faces { get; private set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
    }

    public static class MeshPrefabs
    {
        public static Mesh Cube()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            List<Vertex> vertices = new List<Vertex>();
            vertices.Add(new Vertex(new Vector3(-1, 1, 1), Color.FromArgb(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))));
            vertices.Add(new Vertex(new Vector3(1, 1, 1), Color.FromArgb(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))));
            vertices.Add(new Vertex(new Vector3(-1, -1, 1), Color.FromArgb(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))));
            vertices.Add(new Vertex(new Vector3(1, -1, 1), Color.FromArgb(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))));
            vertices.Add(new Vertex(new Vector3(-1, 1, -1), Color.FromArgb(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))));
            vertices.Add(new Vertex(new Vector3(1, 1, -1), Color.FromArgb(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))));
            vertices.Add(new Vertex(new Vector3(1, -1, -1), Color.FromArgb(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))));
            vertices.Add(new Vertex(new Vector3(-1, -1, -1), Color.FromArgb(r.Next(0, 256), r.Next(0, 256), r.Next(0, 256))));

            List<Face> faces = new List<Face>();
            faces.Add(new Face { A = 0, B = 1, C = 2 });
            faces.Add(new Face { A = 1, B = 2, C = 3 });
            faces.Add(new Face { A = 1, B = 3, C = 6 });
            faces.Add(new Face { A = 1, B = 5, C = 6 });
            faces.Add(new Face { A = 0, B = 1, C = 4 });
            faces.Add(new Face { A = 1, B = 4, C = 5 });
            faces.Add(new Face { A = 2, B = 3, C = 7 });
            faces.Add(new Face { A = 3, B = 6, C = 7 });
            faces.Add(new Face { A = 0, B = 2, C = 7 });
            faces.Add(new Face { A = 0, B = 4, C = 7 });
            faces.Add( new Face { A = 4, B = 5, C = 6 });
            faces.Add( new Face { A = 4, B = 6, C = 7 });

            return new Mesh(vertices, faces);
        }
    }
}