using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class PrintMatrix : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnGUI()
    {
        if (Selection.activeTransform != null)
        {
            Vector3 vec = new Vector3(100, 80, 60);
            GUILayout.Label(Selection.activeTransform.localToWorldMatrix.ToString());
            GUILayout.Label(Selection.activeTransform.TransformPoint(vec).ToString());
            GUILayout.Label((Selection.activeTransform.localToWorldMatrix * vec).ToString());
            GUILayout.Label((Selection.activeTransform.localToWorldMatrix.MultiplyPoint(vec)).ToString());
        }
    }
}
