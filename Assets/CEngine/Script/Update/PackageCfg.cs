//********************************************************************
//	CopyRight   CEngine
//	Purpose:	打包文件类
//	Created:	2020-04-28
//	Author:		ChenTao
//  QQ:         1107689123
//  Mail:       1107689123@qq.com
//********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CEngine
{
    [System.Serializable]
    public class FileCfg
    {
        public string MD5 = "";
        public string Path = "";

        public FileCfg(string md5, string path) { MD5 = md5; Path = path; }
    }

    public class PackageCfg
    { 
        public int CurVersion = 0;
        public int PatchVersion = 0;
        public int ForceUpdateVersion = 0;

        public List<FileCfg> Files = new List<FileCfg>();
    }
}