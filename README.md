# CEngine 是一个轻量级Unity开发框架

CEngine 是一个轻量级Unity开发框架，可满足常用的开发需求，主要包含的功能有:

* AssetBundle打包加载全套逻辑(整合SharpZipLib完成压缩解压)
* AssetBundle管理类
* 整合 IFix 完成代码热更
* 高效时间轮定时器(帧定时器及逻辑定时器)
* UI管理类
* 消息机制
* 对象池重用机制
* 日志输出为文件流保存本地方便调试

## 关于 CEngine 工作流的说明
现有Unity开发工作流基本分为两类，一类在实现业务过程中不需要关心AssetBundle的加载与卸载，利用AssetDataBase实现编辑器模式与AB包的分离，这种开发模式优点在于快速开发，在编辑器上开发时不需要打包，出apk或者ipa时才打包，实现了加载逻辑与业务逻辑的分离，缺点在于AssetBundle不存在卸载逻辑，所有的AB包都会存在于内存，极大的增加了内存使用，最终打包时耗时极长，CEngine 使用了另一类工作流，打包时资源有修改时需要主动打一次AB包，整个开发过程中直接控制AB包的加载卸载，即编辑器与真机的AB加载具有一致性，优点在于主动管理AB包及其依赖的加载和卸载，极大减少内存消耗，打apk或ipa时速度极快，不需要额外打AB包的过程，缺点在于增加了开发复杂度以及开发人员门槛

## AssetBundle开发流程
![Assetbundle](https://note.youdao.com/yws/public/resource/4b7367109b032273069df9c88f971989/xmlnote/WEBRESOURCEa67d4dc29b93b2f5ff00af760d23179e/26)

打包的资源需要放到AssetBundle的子目录下，右键点击 **CreateAssetBundle** 即可完成AB打包，开发过程中打的AB包都会存放在DevCaChe目录，代码获取资源 **AssetBundleMgr.instance.GetAssetBundle("ui/common.unity3d").LoadAsset&#60;GameObject&#62;("ResName")**
代码卸载AB包 **AssetBundleMgr.instance.UnloadAssetBundle("ui/common.unity3d")**

点击 **打开持久化数据目录** 可打开游戏运行时存储数据目录，包括解压完成的AB包和Log日志


## AssetBundle出包流程
![Assetbundle](https://note.youdao.com/yws/public/resource/4b7367109b032273069df9c88f971989/xmlnote/WEBRESOURCEdf07f560d657da4166f4bcd9f4f0ce3a/21)

点击 **拷贝压缩并生成配置** 将会生成AB压缩包到StreamingAssets目录中

![Assetbundle](https://note.youdao.com/yws/public/resource/4b7367109b032273069df9c88f971989/xmlnote/WEBRESOURCE961a823a59e33ea1c61a121bb852f156/29)

## 调用定时器

+ 调用帧定时器 TimerMgr.SetFrameTimer(10, ()=>{ Debug.Log("invoke") }); 10帧后打印invoke
+ 调用逻辑定时器 TimerMgr.SetLogicTimer(10, ()=>{ Debug.Log("invoke") }); 10毫秒后打印invoke

## 发送与接收消息
+ 发送消息
	+ 发送无参数消息 EventMgr.instance.SendEvent("msg");
	+ 发送一个参数消息 EventMgr.instance.SendEvent&#60;float&#62;("msg", 0.1f);
	+ 发送两个参数消息 EventMgr.instance.SendEvent&#60;float,float&#62;("msg", 0.1f, 0.2f);
+ 注册消息[清理时需要反注册，比如界面关闭时]
	+ 注册无参 EventMgr.instance.RegEvent("msg", callback);
	+ 注册一个参数消息 EventMgr.instance.RegEvent&#60;float&#62;("msg", callback1);
	+ 注册两个参数消息 EventMgr.instance.RegEvent&#60;float,float&#62;("msg", callback2);
    + 反注册无参 EventMgr.instance.UnRegEvent("msg", callback);
	+ 反注册一个参数消息 EventMgr.instance.UnRegEvent&#60;float&#62;("msg", callback1);
	+ 反注册两个参数消息 EventMgr.instance.UnRegEvent&#60;float,float&#62;("msg", callback2);

![msg](https://note.youdao.com/yws/public/resource/4b7367109b032273069df9c88f971989/xmlnote/WEBRESOURCE833e5569ef804a17f907cda8208df551/32)

## 对象池机制
**使用 ObjectPool 模板类即可使用对象池**
private ObjectPool<TimerEvent> _teNodePool = new ObjectPool<TimerEvent>()
从对象池中获取对象 _teNodePool.Get()
释放对象回对象池 _teNodePool.Release(te)

## 日志输出为文件流
调试模式下默认开启
![debug](https://note.youdao.com/yws/public/resource/4b7367109b032273069df9c88f971989/xmlnote/WEBRESOURCE149771dd496cdc4965cfc1bb5422b501/37)
