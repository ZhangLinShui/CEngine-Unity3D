//********************************************************************
//	CopyRight   CEngine
//	Purpose:	时间轮定时器基类
//	Created:	2020-04-28
//	Author:		ChenTao
//  QQ:         1107689123
//  Mail:       1107689123@qq.com
//********************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace CEngine
{
    public class LinkedNode
    {
        public LinkedNode Next;
        public LinkedNode Prev;

        public LinkedNode()
        {
            Next = this;
            Prev = this;
        }

        public void AddLast(LinkedNode node)
        {
            node.Next = this;
            node.Prev = Prev;
            Prev.Next = node;
            this.Prev = node;
        }

        public LinkedNode Split()
        {
            var node = Next;

            Prev.Next = Next;
            Next.Prev = Prev;
            Next = this;
            Prev = this;

            return node;
        }

        public void Reset()
        {
            Next = this;
            Prev = this;
        }
    }

    public class TimerEvent : LinkedNode
    {
        public int ExpireTime;
        public Action Callback;
    }

    public class BaseTimer
    {
        public class TimerWheel
        {
            public const int kFirstBit = 8;
            public const int kFirstCount = 1 << kFirstBit;
            public const int kNWheelCount = 3;
            public const int kNWheelBit = 8;
            public const int kNCount = 1 << kNWheelBit;
            public const int kFirstMask = kFirstCount - 1;
            public const int kNMask = kNCount - 1;

            public int _currentTick = 0;
            private LinkedNode[] _firstWheel = new LinkedNode[kFirstCount];
            private LinkedNode[][] _NWheel = new LinkedNode[kNWheelCount][];
            private ObjectPool<TimerEvent> _teNodePool = new ObjectPool<TimerEvent>();

            public TimerWheel()
            {
                for (int i = 0; i < _firstWheel.Length; ++i)
                {
                    _firstWheel[i] = new LinkedNode();
                }
                for (int i = 0; i < kNWheelCount; ++i)
                {
                    _NWheel[i] = new LinkedNode[kNCount];

                    for (int j = 0; j < kNCount; ++j)
                    {
                        _NWheel[i][j] = new LinkedNode();
                    }
                }
            }

            public void AddTickCounter()
            {
                _currentTick++;
            }

            public void SetTimer(int delaytime, Action callback)
            {
                var te = _teNodePool.Get();
                te.Callback = callback;
                te.ExpireTime = _currentTick + delaytime;

                AddTimer(te);
            }

            public void AddTimer(TimerEvent te)
            {
                var delayTime = te.ExpireTime - _currentTick;

                if (delayTime < TimerWheel.kFirstCount)
                {
                    AddFirst(te);
                }
                else
                {
                    int n = 1;
                    var val = GetNthMax(n);
                    while (delayTime >= val && n < kNWheelCount)
                    {
                        val = GetNthMax(++n);
                    }
                    AddToNWheel(te, n);
                }
            }

            public int GetNthIndex(int t, int n)
            {
                return (t >> (kFirstBit + (n - 1) * kNWheelBit)) & kNMask;
            }

            public long GetNthMax(int n)
            {
                return 1 << (kFirstBit + n * kNWheelBit);
            }

            public void AddFirst(TimerEvent te)
            {
                _firstWheel[te.ExpireTime & kFirstMask].AddLast(te);
            }

            public void AddToNWheel(TimerEvent te, int n)
            {
                _NWheel[n - 1][GetNthIndex(te.ExpireTime, n)].AddLast(te);
            }

            private LinkedNode _head = new LinkedNode();
            public void Tick()
            {
                if (0 == (_currentTick & kFirstMask))
                {
                    int i = 0;
                    int index = 0;
                    do
                    {
                        index = GetNthIndex(_currentTick, i + 1);
                        _head = _NWheel[i][index];
                        if (_head != _head.Next)
                        {
                            var node = _head.Split();
                            while (node != node.Next)
                            {
                                var next = node.Split();
                                AddTimer((TimerEvent)node);
                                node = next;
                            }
                            AddTimer((TimerEvent)node);
                        }
                    } while (index == 0 && ++i < kNWheelCount);
                }
                _head = _firstWheel[_currentTick & kFirstMask];
                if (_head != _head.Next)
                {
                    var node = _head;
                    while (_head != node.Next)
                    {
                        var te = (TimerEvent)node.Next;
                        if (null != te.Callback)
                        {
                            te.Callback();
                        }
                        node = node.Next;
                        _teNodePool.Release(te);
                    }
                    _head.Reset();
                }
            }
        }

        public TimerWheel Wheel = new TimerWheel();

        public void SetTimer(int delayTime, Action callback)
        {
            Wheel.SetTimer(delayTime, callback);
        }
    }
}
