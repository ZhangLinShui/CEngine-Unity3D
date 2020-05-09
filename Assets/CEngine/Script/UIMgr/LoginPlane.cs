using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CEngine;
using UnityEngine.UI;

namespace GameLogic
{
    public class LoginPlane : BasePlane
    {
        public Slider _slider;

        public override void OnOpen(object[] args)
        {
            EventMgr.instance.RegEvent<float>(EventDefine.UpdateProgress, OnUpdateProgress);
        }

        public override void OnClear()
        {
            EventMgr.instance.UnRegEvent<float>(EventDefine.UpdateProgress, OnUpdateProgress);
        }

        private void OnUpdateProgress(float progress)
        {
            _slider.value = progress;
        }
    }
}
