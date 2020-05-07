using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CEngine;

namespace GameLogic
{
    public class UIGameLogcConfigMgr : UIConfigMgr
    {
        protected override void OnInit()
        {
            base.OnInit();

            //Config(CUIPlane.EnterUpdatePlane, LayerType.Base, WindowType.FullScreen, ResType.ResourceLoad, OpenType.Back, CacheType.Destroy);
        }
    }

    public class UIPlane : CUIPlane
    {
        //public const string EnterUpdatePlane = "EnterUpdatePlane";
    }
}
