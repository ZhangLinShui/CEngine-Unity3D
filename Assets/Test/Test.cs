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
            UnityEngine.Debug.LogError("xx2");
        }

        int count = 0;
        private void Update()
        {
            count++;
            if (count == 10)
            {
                Debug.LogError("good");
            }
        }
    }
}