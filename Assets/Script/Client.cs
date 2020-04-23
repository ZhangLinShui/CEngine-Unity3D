using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CEngine;

public class Client : SceneTemplate<Client>
{
    protected override void OnAwake()
    {
        GameObject.DontDestroyOnLoad(gameObject);

        LoadMainCanvas();

        gameObject.AddComponent<TimerMgr>();

        AssetBundleMgr.instance.Init();
    }

    private void LoadMainCanvas()
    {
        var mc = Resources.Load<GameObject>("MainCanvas");
        GameObject.Instantiate(mc);
    }

    protected override void OnClear()
    {
        AssetBundleMgr.instance.Dispose();
    }
}
