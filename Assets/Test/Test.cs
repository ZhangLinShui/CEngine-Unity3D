using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using CEngine;
using System.IO;


namespace GameLogic
{
    public class Test
    {
        public static void Add(int a, int b)
        {
            var ret = a * b;
            Debug.LogError(ret);
        }
    }
}