using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Main : MonoBehaviour
{
    public Transform TrRotStart;
    public Transform TrRotEnd;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    
    void DrawSegment(Color color, Quaternion a, Quaternion b, Vector3 baseVector, Vector3 offset)
    {
        Gizmos.color = color;
        Vector3 rotSegment0 = a * baseVector;
        Vector3 rotSegment1 = b * baseVector;
        Gizmos.DrawLine(Vector3.zero + offset, rotSegment0 + offset);
        Gizmos.DrawLine(Vector3.zero + offset, rotSegment1 + offset);
        Gizmos.DrawLine(rotSegment0 + offset, rotSegment1 + offset);
    }



    void DrawSegment(Color color, Vector3 eulerA, Vector3 eulerB, Vector3 baseVector, Vector3 offset)
    {
        Gizmos.color = color;
        Vector3 rotSegment0 = Quaternion.Euler(eulerA) * baseVector;
        Vector3 rotSegment1 = Quaternion.Euler(eulerB) * baseVector;
        Gizmos.DrawLine(Vector3.zero + offset, rotSegment0 + offset);
        Gizmos.DrawLine(Vector3.zero + offset, rotSegment1 + offset);
        Gizmos.DrawLine(rotSegment0 + offset, rotSegment1 + offset);
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = Matrix4x4.identity;

        if (TrRotStart == null || TrRotEnd == null)
        {
            return;
        }

        int segments = 100;

        // 两个姿态之间的差值
        Quaternion quaterDis = TrRotEnd.rotation * Quaternion.Inverse(TrRotStart.rotation);

        // 得到旋转轴
        Vector3 rotAxis;
        float angle;
        quaterDis.ToAngleAxis(out angle, out rotAxis);

        Gizmos.color = Color.white;
        Gizmos.DrawLine(Vector3.zero, rotAxis * 50);

        // 随便找一个垂直于旋转轴的向量
        var a = Vector3.Cross(rotAxis, Vector3.forward);
        var b = Vector3.Cross(rotAxis, Vector3.right);
        var baseVector = a.sqrMagnitude > b.sqrMagnitude ? a : b;
        baseVector = baseVector.normalized * 10;

        baseVector = Quaternion.Inverse(TrRotStart.rotation) * baseVector;

        /*
         * 
        // 轴角递进
        Vector3 offset0 = new Vector3(0.1f, 0.1f, 0.1f);
        for (int i = 0; i < segments; i++)
        {
            var q0 = Quaternion.AngleAxis(angle / segments * i, rotAxis);
            var q1 = Quaternion.AngleAxis(angle / segments * (i + 1), rotAxis);

            DrawSegment(Color.red, q0 * TrRotStart.rotation, q1 * TrRotStart.rotation, baseVector, offset0);
        }

        // 四元差值递进
        Vector3 offset1 = new Vector3(0.0f, 0.1f, 0.1f);
        for (int i = 0; i < segments; i++)
        {
            var q0 = Quaternion.Slerp(Quaternion.identity, quaterDis, i / (float)segments);
            var q1 = Quaternion.Slerp(Quaternion.identity, quaterDis, (i + 1) / (float)segments);
            DrawSegment(Color.green, q0 * TrRotStart.rotation, q1 * TrRotStart.rotation, baseVector, offset1);
        }
        */

        // 四元Slerp递进
        Vector3 offset2 = new Vector3(0.0f, 0.0f, 0.1f);
        for (int i = 0; i < segments; i++)
        {
            var q0 = Quaternion.Slerp(TrRotStart.rotation, TrRotEnd.rotation, i / (float)segments);
            var q1 = Quaternion.Slerp(TrRotStart.rotation, TrRotEnd.rotation, (i+1)/(float)segments);

            DrawSegment(Color.blue, q0, q1, baseVector, offset2);
        }

        // 欧拉差值递进
        for (int i = 0; i < segments; i++)
        {
            var eulur0 = Vector3.Lerp(TrRotStart.eulerAngles, TrRotEnd.eulerAngles, i / (float)segments);
            var eulur1 = Vector3.Lerp(TrRotStart.eulerAngles, TrRotEnd.eulerAngles, (i+1) / (float)segments);
            DrawSegment(Color.red, eulur0, eulur1, baseVector, Vector3.zero);
        }
    }
}
