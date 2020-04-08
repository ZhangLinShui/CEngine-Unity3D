using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CEngine;

public class CEngineMgr : SceneTemplate<CEngineMgr>
{
    protected override void OnInit()
    {
        gameObject.AddComponent<TimerMgr>();
    }
}
