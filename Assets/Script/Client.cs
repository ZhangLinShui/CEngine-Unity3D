using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CEngine;
using System.IO;

namespace GameLogic
{
    public class Client : SceneTemplate<Client>
    {
        public enum DebugMode
        {
            Debug,
            Release
        }

        public enum EPlatform
        {
            Test,
            OnLine
        }

        public DebugMode LogMode = DebugMode.Debug;
        public EPlatform Platform = EPlatform.Test;

        private void OnEnable()
        {
            if (Platform == EPlatform.Test && LogMode != DebugMode.Debug)
                Application.logMessageReceived += OnLog;
        }

        private void OnDisable()
        {
            if (Platform == EPlatform.Test && LogMode != DebugMode.Debug)
                Application.logMessageReceived -= OnLog;
        }

        public void OnLog(string condition, string trackback, LogType lt)
        {
            if (Platform == EPlatform.Test)
            {
                if (lt == LogType.Error || lt == LogType.Exception)
                {
                    if (null == LogMgr.instance)
                    {
                        gameObject.AddComponent<LogMgr>().ForceAddQueue(condition, trackback, lt);
                    }
                }
            }
        }

        protected override void OnInit()
        {
            Application.targetFrameRate = SysConfig.TargetFrame;
            QualitySettings.vSyncCount = 0;

            GameObject.DontDestroyOnLoad(gameObject);

            GameController.instance.Init();
            AssetBundleMgr.instance.Init();
            UIGameLogcConfigMgr.instance.Init();
            EventMgr.instance.Init();
            
            LoadMainCanvas();
            UIMgr.instance.InjectChunkMgr(UIGameLogcConfigMgr.instance);
            UIMgr.instance.OpenUI(UIPlane.LoginPlane);

            gameObject.AddComponent<TimerMgr>();
            gameObject.AddComponent<UpdateMgr>();

            if (LogMode == DebugMode.Debug || File.Exists(Application.persistentDataPath + "/" + LogMgr.MarkFile))
            {
                gameObject.AddComponent<LogMgr>();
            }
        }

        private void LoadMainCanvas()
        {
            var mc = Resources.Load<GameObject>("MainCanvas");
            GameObject.Instantiate(mc);
        }

        protected override void OnClear()
        {
            GameController.instance.Dispose();
            AssetBundleMgr.instance.Dispose();
            UIGameLogcConfigMgr.instance.Dispose();
            EventMgr.instance.Dispose();
        }
    }
}
