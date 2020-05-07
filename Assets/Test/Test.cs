using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using CEngine;
using System.IO;


namespace GameLogic
{
    public class Test : MonoBehaviour
    {
        private void Start()
        {
            var stack = new Stack<int>();
            stack.Push(1);
            stack.Push(2);

            foreach (var d in stack)
            {
                TimeLogger.LogError(d.ToString());
            }
        }

        public void Generic<T>(T a)
        {
        }

        private void Show(int b)
        {
        }
    }
}