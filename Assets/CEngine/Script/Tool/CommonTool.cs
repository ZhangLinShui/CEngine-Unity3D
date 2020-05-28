//********************************************************************
//	CopyRight   CEngine
//	Purpose:	工具类
//	Created:	2020-04-28
//	Author:		ChenTao
//  QQ:         1107689123
//  Mail:       1107689123@qq.com
//********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace CEngine
{
    public static class CommonTool
    {
        public static string CalFileMD5(string path)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    var hash = md5.ComputeHash(stream);

                    var sb = new StringBuilder();
                    foreach (var b in hash)
                    {
                        sb.Append(b.ToString("X2"));
                    }
                    return sb.ToString();
                }
            }
        }
    }
}
