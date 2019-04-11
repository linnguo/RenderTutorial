using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RT.Earth
{
    public class EarthBuilder : MonoBehaviour
    {
        public int xCount = 32;
        public int yCount = 16;

        void Start()
        {
            GetComponent<MeshFilter>().mesh = CreateSphere();
        }

        Mesh CreateSphere()
        {
            Mesh m = new Mesh();

            List<Vector3> vertice = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<int> triangles = new List<int>();

            for (int y = 0; y < yCount + 1; ++y)
            {
                for (int x = 0; x < xCount + 1; ++x)
                {
                    float uvX = 1f / xCount * x;
                    float uvY = 1f / yCount * y;
                    float eulurY = 360f * uvX;
                    float eulurX = 180f * uvY;
                    uvs.Add(new Vector2(uvX, uvY));
                    Vector3 pos = Quaternion.Euler(0, -90 - eulurY, 0) * Quaternion.Euler(eulurX, 0, 0) * new Vector3(0, -1, 0);
                    vertice.Add(pos);
                    normals.Add(pos.normalized);
                }
            }


            for (int y = 0; y < yCount; ++y)
            {
                for (int x = 0; x < xCount; ++x)
                {
                    int leftTop = y * (xCount + 1) + x;
                    int rightTop = leftTop + 1;
                    int leftBottom = leftTop + xCount + 1;
                    int rightBottom = leftBottom + 1;

                    triangles.Add(leftTop);
                    triangles.Add(leftBottom);
                    triangles.Add(rightTop);


                    triangles.Add(leftBottom);
                    triangles.Add(rightBottom);
                    triangles.Add(rightTop);
                }
            }


            m.vertices = vertice.ToArray();
            m.uv = uvs.ToArray();
            m.normals = normals.ToArray();
            m.triangles = triangles.ToArray();

            return m;
        }
    }
}
