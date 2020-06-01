//********************************************************************
//	CopyRight   CEngine
//	Purpose:	角点路径类
//	Created:	2020-06-01
//	Author:		ChenTao
//  QQ:         1107689123
//  Mail:       1107689123@qq.com
//********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerPath : MonoBehaviour
{
    public bool ShowSequence = false;
    [SerializeField]
    private List<Vector3> _path = new List<Vector3>();

    public void AddPoint(Vector3 localPoint)
    {
        localPoint.z = 0f;
        _path.Add(localPoint);
    }

    public List<Vector3> GetPath()
    {
        return _path;
    }

    public void SetPoint(int i, Vector3 pos)
    {
        var p = transform.InverseTransformPoint(pos);
        p.z = 0f;
        _path[i] = p;
    }
}
