using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CEngine;
using System;
using System.Linq;
using System.IO;

namespace CEngine
{
    /// <summary>
    /// UI层类型
    /// </summary>
    public enum LayerType
    {
        Base,
        Common,
        Advance,
    }

    /// <summary>
    /// 窗口类型
    /// </summary>
    public enum WindowType
    {
        PopUp,
        FullScreen,
    }

    /// <summary>
    /// 资源类型
    /// </summary>
    public enum ResType
    {
        ResourceLoad,
        AssetBundleLoad
    }

    /// <summary>
    /// 打开方式
    /// </summary>
    public enum OpenType
    {
        Back,
        NotClose,
    }

    /// <summary>
    /// 缓存方式[ResourceLoad 加载的禁止缓存]
    /// </summary>
    public enum CacheType
    {
        Destroy,
        Cache,
    }

    /// <summary>
    /// 过渡方式(todo)
    /// </summary>
    public enum TransitionType
    {
    }

    /// <summary>
    /// ui 栈数据
    /// </summary>
    public class UIStackChunk
    {
        public string UIPath;
        public BasePlane UIBasePlane;
        public UIConfigChunk ConfigChunk;

        public UIStackChunk(string ui, BasePlane bp, UIConfigChunk cc) { UIPath = ui; UIBasePlane = bp; ConfigChunk = cc; }
    }

    /// <summary>
    /// UI对象池[注意 此部分的ab不会卸载]
    /// </summary>
    public class UIObjectPool
    {
        public Dictionary<string, List<GameObject>> _pool = new Dictionary<string, List<GameObject>>();

        public GameObject Get(string uiPath, UIConfigChunk cc)
        {
            List<GameObject> l;

            if (_pool.TryGetValue(uiPath, out l))
            {
                if (l.Count != 0)
                {
                    l[0].SetActive(true);
                    return l[0];
                }
            }
            else
            {
                _pool[uiPath] = new List<GameObject>();
            }
            GameObject res = AssetBundleMgr.instance.GetAssetBundle(uiPath).LoadAsset<GameObject>(Path.GetFileNameWithoutExtension(uiPath));
            var go = GameObject.Instantiate(res);
            go.name = uiPath;
            return go;
        }

        public void Release(GameObject go)
        {
            go.SetActive(false);
            _pool[go.name].Add(go);
        }
    }

    /// <summary>
    /// UI层
    /// </summary>
    public class UILayer
    {
        public string Name;
        public Transform Origin;

        public UILayer(string n, Transform o) { Name = n;  Origin = o; }

        private UIObjectPool _pool = new UIObjectPool();
        private Stack<UIStackChunk> _stack = new Stack<UIStackChunk>();

        /// <summary>
        /// 回收块
        /// </summary>
        private void Recycle(UIStackChunk stackChunk)
        {
            stackChunk.UIBasePlane.OnClear();
            if (stackChunk.ConfigChunk.UICacheType == CacheType.Cache)
            {
                _pool.Release(stackChunk.UIBasePlane.gameObject);
            }
            else
            {
                GameObject.Destroy(stackChunk.UIBasePlane);
                if (stackChunk.ConfigChunk.UIResType == ResType.AssetBundleLoad)
                {
                    AssetBundleMgr.instance.UnloadAssetBundle(stackChunk.UIPath);
                }
            }
            stackChunk.UIBasePlane = null;
            GC.Collect(0);
        }

        /// <summary>
        /// 打开ui
        /// </summary>
        public void OpenUI(string ui, UIConfigChunk chunk)
        {
            if (chunk.UIResType == ResType.ResourceLoad && chunk.UICacheType == CacheType.Cache)
            {
                TimeLogger.LogError("ui conflict restype and cache type:" + ui);
                return;
            }
            if (_stack.Count != 0)
            {
                var peek = _stack.Peek();
                if (peek.UIPath == ui)
                {
                    return;
                }
                if (peek.ConfigChunk.UIWinType == WindowType.PopUp)
                {
                    _stack.Pop();
                    Recycle(peek);
                }
                else if (peek.ConfigChunk.UIWinType == WindowType.FullScreen)
                {
                    if (peek.ConfigChunk.UIOpenType == OpenType.Back)
                    {
                        Recycle(peek);
                    }
                }
            }
            UIStackChunk cacheStackChunk = null;
            foreach (var stackChunk in _stack)
            {
                if (stackChunk.UIPath == ui && null != stackChunk.UIBasePlane)
                {
                    cacheStackChunk = stackChunk;
                    break;
                }
            }
            BasePlane bp = null;
            if (null == cacheStackChunk)
            {
                GameObject bpGo;
                if (chunk.UIResType == ResType.ResourceLoad)
                {
                    var res = Resources.Load<GameObject>(ui);
                    bpGo = GameObject.Instantiate(res);
                }
                else
                {
                    if (chunk.UICacheType == CacheType.Cache)
                    {
                        bpGo = _pool.Get(ui, chunk);
                    }
                    else
                    {
                        bpGo = AssetBundleMgr.instance.GetAssetBundle(ui).LoadAsset<GameObject>(Path.GetFileNameWithoutExtension(ui));
                    }
                }
                bpGo.transform.parent = Origin;

                bp = bpGo.GetComponent<BasePlane>();
                
                var rt = bp.GetComponent<RectTransform>();
                rt.offsetMax = Vector2.zero;
                rt.offsetMin = Vector2.zero;
                rt.localScale = Vector3.one;
                
                bp.UIPath = ui;
                bp.OnOpen();
            }
            else
            {
                bp = cacheStackChunk.UIBasePlane;
                cacheStackChunk.UIBasePlane = null;
            }
            var uiStackChunk = new UIStackChunk(ui, bp, chunk);
            _stack.Push(uiStackChunk);
        }

        /// <summary>
        /// 关闭 ui
        /// 按照设计规范只允许关闭栈顶ui
        /// </summary>
        public void CloseUI()
        {
            var stackChunk = _stack.Pop();
            stackChunk.UIBasePlane.OnClear();
            Recycle(stackChunk);
            if (stackChunk.ConfigChunk.UIWinType == WindowType.FullScreen)
            {
                if (_stack.Count > 0)
                {
                    var peek = _stack.Peek();
                    if (null == peek.UIBasePlane)
                    {
                        _stack.Pop();
                        OpenUI(peek.UIPath, peek.ConfigChunk);
                        GC.Collect(0);
                    }
                }
            }
        }
    }

    /// <summary>
    /// UI管理器
    /// </summary>
    public class UIMgr : SceneTemplate<UIMgr>
    {
        public UIConfigMgr _uiConfigMgr;
        public List<UILayer> _uiLayers = new List<UILayer>();

        protected override void OnInit()
        {
            foreach (var layer in Enum.GetNames(typeof(LayerType)))
            {
                _uiLayers.Add(new UILayer(layer, transform.Find(layer)));
            }
        }

        private UILayer FindLayer(LayerType lt)
        {
            return _uiLayers.First(n=>n.Name == lt.ToString());
        }

        public void InjectChunkMgr(UIConfigMgr mgr)
        {
            _uiConfigMgr = mgr;
        }

        public void OpenUI(string ui)
        {
            var chunk = _uiConfigMgr.GetUIConfigChunk(ui);
            var layer = FindLayer(chunk.UILayerType);
            layer.OpenUI(ui, chunk);
        }

        public void ClosePeekUI(string ui)
        {
            var chunk = _uiConfigMgr.GetUIConfigChunk(ui);
            var layer = FindLayer(chunk.UILayerType);
            layer.CloseUI();
        }
    }
}
