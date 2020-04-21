using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CEngine
{
    public class MainCanvas : SceneTemplate<MainCanvas>
    {
        protected override void OnAwake()
        {
            var origin = Resources.Load<GameObject>("MainCanvas");
            var mc = GameObject.Instantiate(origin);

            mc.AddComponent<UIMgr>();

            GameObject.DontDestroyOnLoad(mc);
        }
    }
}
