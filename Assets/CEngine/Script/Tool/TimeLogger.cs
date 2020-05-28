//********************************************************************
//	CopyRight   CEngine
//	Purpose:	自定义输出日志类
//	Created:	2020-04-28
//	Author:		ChenTao
//  QQ:         1107689123
//  Mail:       1107689123@qq.com
//********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CEngine
{
    public static class TimeLogger
    {
        private static string sYellow = "#DAA520";

        public static void LogYellow(string str)
        {
            Debug.Log(string.Format("<color={0}>[{1}] {2}</color>", sYellow, DateTime.Now.ToLongTimeString(), str));
        }

        public static void LogError(string str)
        {
            Debug.LogError(string.Format("[{0}] {1}", DateTime.Now.ToLongTimeString(), str));
        }
    }
}