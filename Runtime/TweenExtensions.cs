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
        
        public static TweenAction RotateByX(this Transform transform, float angle, float duration)
        {
            return new TweenAction(transform).RotateBy(Vector3.right, angle, duration);
        }

        public static TweenAction RotateByY(this Transform transform, float angle, float duration)
        {
            return new TweenAction(transform).RotateBy(Vector3.up, angle, duration);
        }

        public static TweenAction RotateByZ(this Transform transform, float angle, float duration)
        {
            return new TweenAction(transform).RotateBy(Vector3.forward, angle, duration);
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