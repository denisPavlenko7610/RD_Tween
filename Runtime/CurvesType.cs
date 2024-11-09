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
        public static float EaseInBack(float t) => t * t * t - t * Mathf.Sin(t * Mathf.PI);
        public static float EaseOutBack(float t) => 1 - Mathf.Pow(1 - t, 3) - Mathf.Sin(t * Mathf.PI);
        public static float EaseInOutBack(float t) => t < 0.5 ? (Mathf.Pow(2 * t, 2) * ((2 * t) * (1.70158f) - 2)) : (Mathf.Pow(2 * t - 2, 2) * ((2 * t - 2) * (1.70158f) + 2) + 2) / 2;
    }
}

