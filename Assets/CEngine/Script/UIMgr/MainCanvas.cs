using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CEngine
{
    public class MainCanvas : SceneTemplate<MainCanvas>
    {
        protected override void OnInit()
        {
            GameObject.DontDestroyOnLoad(this);
        }
    }
}
