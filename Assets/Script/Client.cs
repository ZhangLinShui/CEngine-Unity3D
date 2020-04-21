using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CEngine;

public class Client : SceneTemplate<Client>
{
    protected override void OnAwake()
    {
        GameObject.DontDestroyOnLoad(gameObject);

        gameObject.AddComponent<TimerMgr>();
        gameObject.AddComponent<MainCanvas>();

        AssetBundleMgr.instance.Init();
    }

    protected override void OnClear()
    {
        AssetBundleMgr.instance.Dispose();
    }
}
