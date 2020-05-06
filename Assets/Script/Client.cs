using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CEngine;

namespace GameLogic
{
    public class Client : SceneTemplate<Client>
    {
        protected override void OnInit()
        {
            Application.targetFrameRate = SysConfig.TargetFrame;

            GameObject.DontDestroyOnLoad(gameObject);

            LoadMainCanvas();

            gameObject.AddComponent<TimerMgr>();
            gameObject.AddComponent<UpdateMgr>();

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
}
