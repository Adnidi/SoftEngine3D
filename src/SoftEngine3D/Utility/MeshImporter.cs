using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SoftEngine3D.Primitives;

namespace SoftEngine3D.Utility
{
    public static class MeshImporter
    {
        public static Mesh[] LoadBabylonFile(string fileName)
        {
            var meshes = new List<Mesh>();
            var data = File.ReadAllText(fileName);
            dynamic jsonObject = JsonConvert.DeserializeObject(data);

            foreach (var meshObject in jsonObject.meshes)
            {
                var verticesArray = meshObject.positions;
                var colorArray = meshObject.colors;
                // Faces
                var indicesArray = meshObject.indices;

                var verticesStep = 3;

                // the number of interesting vertices information for us
                var verticesCount = verticesArray.Count / verticesStep;
                // number of faces is logically the size of the array divided by 3 (A, B, C)
                var facesCount = indicesArray.Count / 3;
                var mesh = new Mesh();

                // Filling the Vertices array of our mesh first
                for (var index = 0; index < verticesCount; index++)
                {
                    var x = (float)verticesArray[index * verticesStep].Value;
                    var y = -(float)verticesArray[index * verticesStep + 1].Value;
                    var z = (float)verticesArray[index * verticesStep + 2].Value;

                    var r = (int)(colorArray[index * 4].Value * 255);
                    var b = (int)(colorArray[index * 4 + 1].Value * 255);
                    var g = (int)(colorArray[index * 4 + 2].Value * 255);
                    var alpha = (int)(colorArray[index * 4 + 3].Value * 255);

                    var color = Color.FromArgb(
                        alpha,
                        r,
                        b,
                        g);

                    mesh.Vertices.Add(new Vertex(new Vector3(x, y, z), color));
                }

                // Then filling the Faces array
                for (var index = 0; index < facesCount; index++)
                {
                    var a = (int)indicesArray[index * 3].Value;
                    var b = (int)indicesArray[index * 3 + 1].Value;
                    var c = (int)indicesArray[index * 3 + 2].Value;
                    mesh.Faces.Add(new Face { A = a, B = b, C = c });
                }

                // Getting the position you've set in Blender
                var position = meshObject.position;
                mesh.Position = new Vector3((float)position[0].Value, (float)position[1].Value, (float)position[2].Value);
                meshes.Add(mesh);
            }
            return meshes.ToArray();
        }
    }
}