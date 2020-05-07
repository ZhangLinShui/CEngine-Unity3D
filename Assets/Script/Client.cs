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

            AssetBundleMgr.instance.Init();
            UIGameLogcConfigMgr.instance.Init();
            EventListener.instance.Init();
            
            LoadMainCanvas();
            UIMgr.instance.InjectChunkMgr(UIGameLogcConfigMgr.instance);
            UIMgr.instance.OpenUI(UIPlane.LoginPlane);

            gameObject.AddComponent<TimerMgr>();
            gameObject.AddComponent<UpdateMgr>();
        }

        private void LoadMainCanvas()
        {
            var mc = Resources.Load<GameObject>("MainCanvas");
            GameObject.Instantiate(mc);
        }

        protected override void OnClear()
        {
            AssetBundleMgr.instance.Dispose();
            UIGameLogcConfigMgr.instance.Dispose();
            EventListener.instance.Dispose();
        }
    }
}
