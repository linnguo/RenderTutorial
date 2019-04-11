using UnityEngine;

/// <summary>
/// 自己动手实现SkinMesh
/// </summary>
public class SkinMeshGPUBinding : MonoBehaviour
{
    public SkinnedMeshRenderer SkinnedMeshRenderer;
    public MeshFilter MeshFilter;
    public Renderer Renderer;
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

        LocalPos0 = new Vector3[VertexCount];
        LocalPos1 = new Vector3[VertexCount];
        LocalPos2 = new Vector3[VertexCount];
        LocalPos3 = new Vector3[VertexCount];

        for (int i = 0; i < VertexCount; ++i)
        {
            LocalPos0[i] = OriMesh.bindposes[OriMesh.boneWeights[i].boneIndex0].MultiplyPoint(OriMesh.vertices[i]);
            LocalPos1[i] = OriMesh.bindposes[OriMesh.boneWeights[i].boneIndex1].MultiplyPoint(OriMesh.vertices[i]);
            LocalPos2[i] = OriMesh.bindposes[OriMesh.boneWeights[i].boneIndex2].MultiplyPoint(OriMesh.vertices[i]);
            LocalPos3[i] = OriMesh.bindposes[OriMesh.boneWeights[i].boneIndex3].MultiplyPoint(OriMesh.vertices[i]);
        }

        MyMesh.vertices = LocalPos0;
        MyMesh.normals = LocalPos1;

        MyMesh.uv = (Vector2[])OriMesh.uv.Clone();
        MyMesh.triangles = (int[])OriMesh.triangles.Clone();

        Vector2[] Bone1 = new Vector2[VertexCount];
        Vector2[] Bone2 = new Vector2[VertexCount];

        for (int i = 0; i < VertexCount; ++i)
        {
            Bone1[i] = new Vector2(OriMesh.boneWeights[i].boneIndex0, OriMesh.boneWeights[i].weight0);
            Bone2[i] = new Vector2(OriMesh.boneWeights[i].boneIndex1, OriMesh.boneWeights[i].weight1);
        }

        MyMesh.uv2 = Bone1;
        MyMesh.uv3 = Bone2;

        int BoneCount = SkinnedMeshRenderer.bones.Length;
        Matrix4x4[] bones = new Matrix4x4[BoneCount];
        for (int i = 0; i < BoneCount; ++i)
        {
            bones[i] = SkinnedMeshRenderer.bones[i].localToWorldMatrix;
        }
        Renderer.material.SetMatrixArray("_Bones", bones);
    }

}
