using System;
using UnityEngine;

namespace RD_Tween.Runtime
{
    public class TweenUpdater : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            TweenManager.Instance.Update(Time.deltaTime);
        }
    }
}