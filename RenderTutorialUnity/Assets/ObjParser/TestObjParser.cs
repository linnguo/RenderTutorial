using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObjParser : MonoBehaviour
{

    public TextAsset obj;
    public MeshFilter filter;

    void Start()
    {
        if (filter != null && obj != null)
        {
            filter.mesh = ObjParser.ParseObj(obj.text);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
