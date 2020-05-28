//********************************************************************
//	CopyRight   CEngine
//	Purpose:	对象池类
//	Created:	2020-04-28
//	Author:		ChenTao
//  QQ:         1107689123
//  Mail:       1107689123@qq.com
//********************************************************************
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 对象池
/// </summary>
namespace CEngine
{
    /// <summary>
    /// C#托管对象类[继承自object 但不继承Object]
    /// </summary>
    public class ObjectPool<T> where T : new()
    {
        private List<T> _objects = new List<T>();

        public virtual T Get()
        {
            if (0 != _objects.Count)
            {
                var obj = _objects[0];
                _objects.RemoveAt(0);
                return obj;
            }
            return new T();
        }

        public virtual void Release(T obj)
        {
            _objects.Add(obj);
        }
    }
}
