using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjParser
{
    public static Mesh ParseObj(string objFileContent)
    {
        var lines = objFileContent.Split('\n');
        List<Vector3> srcPos = new List<Vector3>();
        List<Vector2> srcUV = new List<Vector2>();
        List<Vector3> srcNor = new List<Vector3>();

        List<Vector3> dstPos = new List<Vector3>();
        List<Vector2> dstUV = new List<Vector2>();
        List<Vector3> dstNor = new List<Vector3>();
        List<int> indices = new List<int>();

        foreach (var line in lines)
        {
            if (line.StartsWith("v "))
            {
                var nums = line.Split(' ');
                Debug.Assert(nums.Length == 4);
                Vector3 pos = new Vector3();
                pos.x = -float.Parse(nums[1]);
                pos.y = float.Parse(nums[2]);
                pos.z = float.Parse(nums[3]);
                srcPos.Add(pos);
            }
            else if (line.StartsWith("vt "))
            {
                var nums = line.Split(' ');
                Debug.Assert(nums.Length == 3);
                Vector2 uv = new Vector2();
                uv.x = float.Parse(nums[1]);
                uv.y = float.Parse(nums[2]);
                srcUV.Add(uv);
            }
            else if (line.StartsWith("vn "))
            {
                var nums = line.Split(' ');
                Debug.Assert(nums.Length == 4);
                Vector3 nor = new Vector3();
                nor.x = -float.Parse(nums[1]);
                nor.y = float.Parse(nums[2]);
                nor.z = float.Parse(nums[3]);
                srcNor.Add(nor);
            }
            else if (line.StartsWith("f "))
            {
                int beginIdx = dstPos.Count;

                var vertices = line.Split(' ');
                for (int i = 1; i < vertices.Length; ++i)
                {
                    var vertex = vertices[i];
                    var elems = vertex.Split('/');
                    int posIndex = int.Parse(elems[0]) - 1;
                    int uvIndex = int.Parse(elems[1]) - 1;
                    int norIndex = int.Parse(elems[2]) - 1;

                    dstPos.Add(srcPos[posIndex]);
                    dstUV.Add(srcUV[uvIndex]);
                    dstNor.Add(srcNor[norIndex]);
                }


                for (int i = 0; i < vertices.Length - 3; ++i)
                {
                    indices.Add(beginIdx);
                    indices.Add(beginIdx + i + 2);
                    indices.Add(beginIdx + i + 1);
                }
            }
        }

        Mesh mesh = new Mesh();
        mesh.SetVertices(dstPos);
        mesh.SetUVs(0, dstUV);
        mesh.SetNormals(dstNor);
        mesh.SetTriangles(indices, 0);
        return mesh;
    }
}
