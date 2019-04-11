using UnityEngine;

/// <summary>
/// 自己动手实现SkinMesh
/// </summary>
public class SkinMeshCPUBinding : MonoBehaviour
{
    public SkinnedMeshRenderer SkinnedMeshRenderer;
    public MeshFilter MeshFilter;
    public Mesh OriMesh;

    Mesh MyMesh;
    Vector3[] Vertices;

    Vector3[] LocalPos0;
    Vector3[] LocalPos1;
    Vector3[] LocalPos2;
    Vector3[] LocalPos3;


    void Start()
    {
        int VertexCount = OriMesh.vertexCount;

        MyMesh = new Mesh();

        MeshFilter.mesh = MyMesh;

        Vertices = new Vector3[VertexCount];

        LocalPos0 = new Vector3[VertexCount];
        LocalPos1 = new Vector3[VertexCount];
        LocalPos2 = new Vector3[VertexCount];
        LocalPos3 = new Vector3[VertexCount];

        for (int i = 0; i < VertexCount; ++i)
        {
            Vertices[i] = OriMesh.vertices[i];

            LocalPos0[i] = OriMesh.bindposes[OriMesh.boneWeights[i].boneIndex0].MultiplyPoint(OriMesh.vertices[i]);
            LocalPos1[i] = OriMesh.bindposes[OriMesh.boneWeights[i].boneIndex1].MultiplyPoint(OriMesh.vertices[i]);
            LocalPos2[i] = OriMesh.bindposes[OriMesh.boneWeights[i].boneIndex2].MultiplyPoint(OriMesh.vertices[i]);
            LocalPos3[i] = OriMesh.bindposes[OriMesh.boneWeights[i].boneIndex3].MultiplyPoint(OriMesh.vertices[i]);
        }

        MyMesh.vertices = Vertices;
        MyMesh.uv = (Vector2[])OriMesh.uv.Clone();
        MyMesh.colors = (Color[])OriMesh.colors.Clone();
        MyMesh.triangles = (int[])OriMesh.triangles.Clone();

        Refresh();
    }

    void Refresh()
    {
        int[] BoneIndice = new int[4];
        float[] Weightes = new float[4];

        int VertexCount = OriMesh.vertexCount;
        for (int i = 0; i < VertexCount; ++i)
        {
            BoneIndice[0] = OriMesh.boneWeights[i].boneIndex0;
            BoneIndice[1] = OriMesh.boneWeights[i].boneIndex1;
            BoneIndice[2] = OriMesh.boneWeights[i].boneIndex2;
            BoneIndice[3] = OriMesh.boneWeights[i].boneIndex3;
            Weightes[0] = OriMesh.boneWeights[i].weight0;
            Weightes[1] = OriMesh.boneWeights[i].weight1;
            Weightes[2] = OriMesh.boneWeights[i].weight2;
            Weightes[3] = OriMesh.boneWeights[i].weight3;


            //Vertices[i] = SkinnedMeshRenderer.bones[BoneIndice[0]].TransformPoint(LocalPos0[i]) * Weightes[0] +
            //  SkinnedMeshRenderer.bones[BoneIndice[1]].TransformPoint(LocalPos1[i]) * Weightes[1] +
            //  SkinnedMeshRenderer.bones[BoneIndice[2]].TransformPoint(LocalPos2[i]) * Weightes[2] +
            //  SkinnedMeshRenderer.bones[BoneIndice[3]].TransformPoint(LocalPos3[i]) * Weightes[3];

            Vertices[i] = Trans(SkinnedMeshRenderer.bones[BoneIndice[0]], LocalPos0[i]) * Weightes[0] +
                Trans(SkinnedMeshRenderer.bones[BoneIndice[1]], LocalPos1[i]) * Weightes[1]/* +
                Trans(SkinnedMeshRenderer.bones[BoneIndice[2]], LocalPos2[i]) * Weightes[2] +
                Trans(SkinnedMeshRenderer.bones[BoneIndice[3]], LocalPos3[i]) * Weightes[3]*/;

        }

        MyMesh.vertices = Vertices;
    }

    Vector3 Trans(Transform tran, Vector3 vecIn)
    {
        // 正确
        // return tran.TransformPoint(vecIn);

        // 正确
        // Vector4 v4 = tran.localToWorldMatrix * new Vector4(vecIn.x, vecIn.y, vecIn.z, 1);
        // return new Vector3(v4.x/v4.w, v4.y/v4.w, v4.z/v4.w);


        // 正确
        Vector4 v4 = tran.localToWorldMatrix * new Vector4(vecIn.x, vecIn.y, vecIn.z, 1);
        return new Vector3(v4.x, v4.y, v4.z);

        // 错误
        // Vector4 v4 = tran.localToWorldMatrix * new Vector4(vecIn.x, vecIn.y, vecIn.z, 0);
        // return new Vector3(v4.x, v4.y, v4.z);

        // 正确
        // return tran.localToWorldMatrix.MultiplyPoint(vecIn);

        // 错误
        // return tran.localToWorldMatrix * vecIn;


    }

    void Update()
    {
        // Refresh();
    }
}
