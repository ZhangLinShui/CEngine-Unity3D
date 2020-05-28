using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CEngine;
using UnityEngine.SceneManagement;

namespace GameLogic
{
    /// <summary>
    /// 游戏管理器
    /// </summary>
    public class GameController : MgrTemplate<GameController>
    {
        protected override void OnInit()
        {
            EventMgr.instance.RegEvent(EventDefine.UpdateFinish, OnUpdateFinish);
        }

        private void OnUpdateFinish()
        {
            SceneManager.LoadScene(SceneConfig.LevelOne);
        }

        protected override void OnClear()
        {
            EventMgr.instance.UnRegEvent(EventDefine.UpdateFinish, OnUpdateFinish);
        }
    }
}