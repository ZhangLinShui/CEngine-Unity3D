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
        public Transform Target;

        private void Start()
        {
        }

        private void Update()
        {
            var dir = Target.position - transform.position;

            Quaternion q = new Quaternion();
            q.SetLookRotation(dir, new Vector3(1, 1, 0));
            transform.rotation = q;
        }
    }
}