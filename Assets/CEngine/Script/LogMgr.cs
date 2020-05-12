using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.Profiling;

/// <summary>
/// 日志系统
/// </summary>
namespace CEngine
{
    public class LogMgr : SceneTemplate<LogMgr>
    {
        GUIStyle _debugStyle = new GUIStyle();
        GUIStyle _exceptionStyle = new GUIStyle();

        private float _frame = 0;
        private float _averageFrame = 0;
        private const int kAverageFrame = 30;
        private Queue<float> _averageFrameQueue = new Queue<float>(kAverageFrame);
        private Queue<string> _logQueue = new Queue<string>();

        private StreamWriter _logStreamWriter;

        private string _ErrorInfo;

        public const string MarkFile ="MarkFile";

        protected override void OnInit()
        {
            _debugStyle.normal.textColor = Color.green;
            _debugStyle.fontSize = 22;

            _exceptionStyle.normal.textColor = Color.yellow;
            _exceptionStyle.fontSize = 20;
        }

        private void OnInitLogFile()
        {
            if (null == _logStreamWriter)
            {
                var logDirectory = Application.persistentDataPath + "/" + "Log";
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }
                _logStreamWriter = File.CreateText(logDirectory + "/" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".log");
            }
        }

        protected override void OnClear()
        {
            if (null != _logStreamWriter)
            {
                _logStreamWriter.Dispose();
            }
        }

        private void OnLog(string condition, string traceback, LogType lt)
        {
            var str = condition + " " + traceback;
            if (lt == LogType.Error || lt == LogType.Exception)
            {
                _ErrorInfo = str;
            }
            _logQueue.Enqueue(str);
        }

        public void ForceAddQueue(string condition, string traceback, LogType lt)
        {
            OnLog(condition, traceback, lt);
        }

        private void OnEnable()
        {
            Application.logMessageReceived += OnLog;
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= OnLog;
        }

        private void Update()
        {
            _frame = 1 / Time.deltaTime;
            _averageFrame = GetAverageFrame(_frame);

            if (_logQueue.Count > 0)
            {
                OnInitLogFile();

                _logStreamWriter.Write(_logQueue.Dequeue());
            }
        }

        private float GetAverageFrame(float frame)
        {
            _averageFrameQueue.Enqueue(frame);
            if (_averageFrameQueue.Count > kAverageFrame)
            {
                _averageFrameQueue.Dequeue();
            }
            var count = 0;
            var total = 0f;
            foreach (var f in _averageFrameQueue)
            {
                total += f;
                count++;
            }
            if (count == 0)
            {
                return SysConfig.TargetFrame;
            }
            return total / count;
        }

        private void OnGUI()
        {
            GUILayout.Label("当前帧率:" + _frame.ToString("N2") + "fps", _debugStyle);
            GUILayout.Label("平均帧率:" + _averageFrame.ToString("N2") + "fps", _debugStyle);

            GUILayout.Label("总内存:" + FromByteToMB(Profiler.GetTotalReservedMemoryLong()).ToString("N2") + "MB", _debugStyle);
            GUILayout.Label("使用内存:" + FromByteToMB(Profiler.GetTotalAllocatedMemoryLong()).ToString("N2") + "MB", _debugStyle);
            GUILayout.Label("未使用内存:" + FromByteToMB(Profiler.GetTotalUnusedReservedMemoryLong()).ToString("N2") + "MB", _debugStyle);

            if (!string.IsNullOrEmpty(_ErrorInfo))
            {
                GUILayout.Label(_ErrorInfo, _exceptionStyle);

                if (GUILayout.Button("清除警告"))
                {
                    _ErrorInfo = "";
                }
            }
        }

        private float FromByteToMB(long num)
        {
            return num / (1024 * 1000f);
        }
    }
}
