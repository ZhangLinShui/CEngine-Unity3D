using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CEngine
{
    /// <summary>
    /// UI配置块
    /// </summary>
    public class UIConfigChunk
    {
        public LayerType UILayerType;
        public WindowType UIWinType;
        public ResType UIResType;
        public OpenType UIOpenType;
        public CacheType UICacheType;

        public UIConfigChunk(LayerType lt, WindowType wt, ResType rt, OpenType ot, CacheType ct){ UILayerType = lt; UIWinType = wt; UIResType = rt; UIOpenType = ot; UICacheType = ct;}
    }

    /// <summary>
    /// UI配置块管理器
    /// </summary>
    public class UIConfigMgr : MgrTemplate<UIConfigMgr>
    {
        public Dictionary<string, UIConfigChunk> _uiConfigChunkDict = new Dictionary<string, UIConfigChunk>();

        protected override void OnInit()
        {
            Config(CUIPlane.LoginPlane, LayerType.Common, WindowType.FullScreen, ResType.ResourceLoad, OpenType.Back, CacheType.Destroy);
        }

        protected void Config(string uiPath, LayerType lt, WindowType wt, ResType rt, OpenType ot, CacheType ct)
        {
            _uiConfigChunkDict[uiPath] = new UIConfigChunk(lt, wt, rt, ot, ct);
        }

        public UIConfigChunk GetUIConfigChunk(string ui)
        {
            return _uiConfigChunkDict[ui];
        }
    }

    /// <summary>
    /// 资源路径配置
    /// </summary>
    public class CUIPlane
    {
        public const string LoginPlane = "LoginPlane";
    }
}
