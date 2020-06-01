using System;
using UnityEngine;

[Serializable]
public struct ControlPoint
{
    public Vector2 position;
    public Vector2 tangentBack;
    public Vector2 tangentFront;
    public bool smooth;

    static public ControlPoint MovePosition(ControlPoint pt, Vector2 newPos)
    {
        var newPt = pt;
        newPt.position = newPos;
        return newPt;
    }

    static public ControlPoint MoveTangentBack(ControlPoint pt, Vector2 newTanBack)
    {
        var newPt = pt;
        newPt.tangentBack = newTanBack;
        if (pt.smooth) newPt.tangentFront = pt.tangentFront.magnitude * -newTanBack.normalized;
        return newPt;
    }
    static public ControlPoint MoveTangentFront(ControlPoint pt, Vector2 newTanFront)
    {
        var newPt = pt;
        newPt.tangentFront = newTanFront;
        if (pt.smooth) newPt.tangentBack = pt.tangentBack.magnitude * -newTanFront.normalized;
        return newPt;
    }

    static public ControlPoint WithSmooth(ControlPoint pt, bool smooth)
    {
        var newPt = pt;
        if (smooth != pt.smooth) newPt.tangentBack = -pt.tangentFront;
        return newPt;

    }

    public ControlPoint(Vector2 position, Vector2 tanBack, Vector2 tanFront, bool smooth = false)
    {
        this.position = position;
        this.tangentBack = tanBack;
        this.tangentFront = tanFront;
        this.smooth = smooth;
    }
}