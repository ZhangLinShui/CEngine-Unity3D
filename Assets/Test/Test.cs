using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using CEngine;
using System.IO;


namespace GameLogic
{
    public class IEnu : IEnumerator
    {
        private float t = Time.realtimeSinceStartup;

        public object Current
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool MoveNext()
        {
            if (Time.realtimeSinceStartup - t < 3f)
            {
                return true;
            }
            return false;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }

    public class Test : MonoBehaviour
    {
        private void Start()
        {
            var t = new IEnu();
            while (t.MoveNext())
            {
                ;
            }
            Debug.LogError("ok");
        }
    }
}