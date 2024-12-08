using UnityEngine;

namespace RD_Tween.Runtime
{
    public class TweenUpdater : MonoBehaviour
    {
        void Update()
        {
            Tween.Instance.Update(Time.deltaTime);
        }
    }
}