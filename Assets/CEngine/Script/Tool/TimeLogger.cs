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
    }
}