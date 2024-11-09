using UnityEngine;

namespace RD_Tween.Runtime
{
    public static class TweenExtensions
    {
        public static TweenAction MoveTo(this Transform transform, Vector3 target, float duration)
        {
            var tween = new TweenAction(transform).MoveTo(target, duration);
            return tween;
        }

        public static TweenAction RotateTo(this Transform transform, Vector3 target, float duration)
        {
            var tween = new TweenAction(transform).RotateTo(target, duration);
            return tween;
        }

        public static TweenAction ScaleTo(this Transform transform, Vector3 target, float duration)
        {
            var tween = new TweenAction(transform).ScaleTo(target, duration);
            return tween;
        }
        
        public static TweenAction JumpTo(this Transform transform, Vector3 target, float duration, float jumpHeight)
        {
            var tween = new TweenAction(transform).JumpTo(target, duration, jumpHeight);
            return tween;
        }
    }
}