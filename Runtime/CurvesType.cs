using UnityEngine;

namespace RD_Tween.Runtime
{
    public static class CurvesType
    {
        // Linear
        public static float Linear(float t) => t;

        // Quadratic Easing
        public static float EaseInQuad(float t) => t * t;
        public static float EaseOutQuad(float t) => -t * (t - 2);
        public static float EaseInOutQuad(float t) => t < 0.5 ? 2 * t * t : -1 + (4 - 2 * t) * t;

        // Cubic Easing
        public static float EaseInCubic(float t) => t * t * t;
        public static float EaseOutCubic(float t) => 1 - Mathf.Pow(1 - t, 3);
        public static float EaseInOutCubic(float t) => t < 0.5 ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2;

        // Quartic Easing
        public static float EaseInQuart(float t) => t * t * t * t;
        public static float EaseOutQuart(float t) => 1 - Mathf.Pow(1 - t, 4);
        public static float EaseInOutQuart(float t) => t < 0.5 ? 8 * t * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 4) / 2;

        // Quintic Easing
        public static float EaseInQuint(float t) => t * t * t * t * t;
        public static float EaseOutQuint(float t) => 1 - Mathf.Pow(1 - t, 5);
        public static float EaseInOutQuint(float t) => t < 0.5 ? 16 * t * t * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 5) / 2;

        // Sine Easing
        public static float EaseInSine(float t) => 1 - Mathf.Cos(t * Mathf.PI / 2);
        public static float EaseOutSine(float t) => Mathf.Sin(t * Mathf.PI / 2);
        public static float EaseInOutSine(float t) => -(Mathf.Cos(Mathf.PI * t) - 1) / 2;

        // Exponential Easing
        public static float EaseInExpo(float t) => t == 0 ? 0 : Mathf.Pow(2, 10 * (t - 1));
        public static float EaseOutExpo(float t) => t == 1 ? 1 : 1 - Mathf.Pow(2, -10 * t);
        public static float EaseInOutExpo(float t) => t == 0 ? 0 : t == 1 ? 1 : t < 0.5 ? Mathf.Pow(2, 20 * t - 10) / 2 : (2 - Mathf.Pow(2, -20 * t + 10)) / 2;

        // Circular Easing
        public static float EaseInCirc(float t) => 1 - Mathf.Sqrt(1 - Mathf.Pow(t, 2));
        public static float EaseOutCirc(float t) => Mathf.Sqrt(1 - Mathf.Pow(t - 1, 2));
        public static float EaseInOutCirc(float t) => t < 0.5 ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * t, 2))) / 2 : (Mathf.Sqrt(1 - Mathf.Pow(-2 * t + 2, 2)) + 1) / 2;

        // Elastic Easing
        public static float EaseInElastic(float t) => t == 0 || t == 1 ? t : Mathf.Pow(2, 10 * (t - 1)) * Mathf.Sin((t - 1.1f) * 5 * Mathf.PI);
        public static float EaseOutElastic(float t) => t == 0 || t == 1 ? t : Mathf.Pow(2, -10 * t) * Mathf.Sin((t - 0.1f) * 5 * Mathf.PI) + 1;
        public static float EaseInOutElastic(float t) => t == 0 || t == 1 ? t : t < 0.5 ? Mathf.Pow(2, 20 * t - 10) * Mathf.Sin((20 * t - 10.75f) * Mathf.PI) / 2 : Mathf.Pow(2, -20 * t + 10) * Mathf.Sin((20 * t - 10.75f) * Mathf.PI) / 2 + 1;

        // Back Easing
        private const float BackC1 = 1.70158f;
        private const float BackC2 = BackC1 * 1.525f;

        public static float EaseInBack(float t)
        {
            const float c3 = BackC1 + 1f;
            return c3 * t * t * t - BackC1 * t * t;
        }

        public static float EaseOutBack(float t)
        {
            const float c3 = BackC1 + 1f;
            return 1f + c3 * Mathf.Pow(t - 1f, 3f) + BackC1 * Mathf.Pow(t - 1f, 2f);
        }

        public static float EaseInOutBack(float t)
        {
            return t < 0.5f
                ? (Mathf.Pow(2f * t, 2f) * ((BackC2 + 1f) * 2f * t - BackC2)) / 2f
                : (Mathf.Pow(2f * t - 2f, 2f) * ((BackC2 + 1f) * (t * 2f - 2f) + BackC2) + 2f) / 2f;
        }

        // Bounce Easing
        public static float EaseInBounce(float t) => 1f - EaseOutBounce(1f - t);

        public static float EaseOutBounce(float t)
        {
            const float n1 = 7.5625f;
            const float d1 = 2.75f;

            if (t < 1f / d1)
                return n1 * t * t;
            if (t < 2f / d1)
                return n1 * (t -= 1.5f / d1) * t + 0.75f;
            if (t < 2.5f / d1)
                return n1 * (t -= 2.25f / d1) * t + 0.9375f;
            return n1 * (t -= 2.625f / d1) * t + 0.984375f;
        }

        public static float EaseInOutBounce(float t)
        {
            return t < 0.5f
                ? (1f - EaseOutBounce(1f - 2f * t)) / 2f
                : (1f + EaseOutBounce(2f * t - 1f)) / 2f;
        }

        // Flash / Punch helper
        public static float Punch(float amplitude, float t)
        {
            if (t <= 0f) return 0f;
            if (t >= 1f) return 0f;

            const float p = 9f;
            float s = p * t;
            return amplitude * Mathf.Pow(2f, -10f * t) * Mathf.Sin(s * Mathf.PI * 2f);
        }
    }
}
