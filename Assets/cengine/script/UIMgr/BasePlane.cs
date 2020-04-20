using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CEngine;

public class BasePlane : MonoBehaviour
{
    public string AssetBundleKey;

    public virtual void OnOpen()
    {
    }

    public virtual void OnClear()
    {
    }

    public void CloseMySelf()
    {
        UIMgr.instance.CloseUI(AssetBundleKey);
    }
}
