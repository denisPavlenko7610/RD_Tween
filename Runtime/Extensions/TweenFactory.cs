using System;
using UnityEngine;

namespace RD_Tween.Runtime
{
	public static class TweenFactory
	{
		public static PropertyTween<float> To(
			System.Object target,
			Func<float> getter,
			Action<float> setter,
			float end,
			float duration,
			bool autoPlay = true)
		{
			return PropertyTween<float>.Rent(
				target, autoPlay,
				getter, setter, end, duration,
				Mathf.LerpUnclamped,
				add: (a, b) => a + b,
				sub: (a, b) => a - b,
				mul: (a, f) => a * f
			);
		}

		public static PropertyTween<Vector3> To(
			System.Object target,
			Func<Vector3> getter,
			Action<Vector3> setter,
			Vector3 end,
			float duration,
			bool autoPlay = true)
		{
			return PropertyTween<Vector3>.Rent(
				target, autoPlay,
				getter, setter, end, duration,
				Vector3.LerpUnclamped,
				add: (a, b) => a + b,
				sub: (a, b) => a - b,
				mul: (a, f) => a * f
			);
		}

		public static PropertyTween<Vector2> To(
			System.Object target,
			Func<Vector2> getter,
			Action<Vector2> setter,
			Vector2 end,
			float duration,
			bool autoPlay = true)
		{
			return PropertyTween<Vector2>.Rent(
				target, autoPlay,
				getter, setter, end, duration,
				Vector2.LerpUnclamped,
				add: (a, b) => a + b,
				sub: (a, b) => a - b,
				mul: (a, f) => a * f
			);
		}

		public static PropertyTween<Color> To(
			System.Object target,
			Func<Color> getter,
			Action<Color> setter,
			Color end,
			float duration,
			bool autoPlay = true)
		{
			return PropertyTween<Color>.Rent(
				target, autoPlay,
				getter, setter, end, duration,
				Color.LerpUnclamped,
				add: (a, b) => a + b,
				sub: (a, b) => a - b,
				mul: (a, f) => a * f
			);
		}

		public static PropertyTween<Quaternion> To(
			System.Object target,
			Func<Quaternion> getter,
			Action<Quaternion> setter,
			Quaternion end,
			float duration,
			bool autoPlay = true)
		{
			return PropertyTween<Quaternion>.Rent(
				target, autoPlay,
				getter, setter, end, duration,
				Quaternion.LerpUnclamped,
				add: (a, b) => a * b,
				sub: (a, b) => Quaternion.Inverse(b) * a,
				mul: (a, f) => Quaternion.SlerpUnclamped(Quaternion.identity, a, f)
			);
		}
	}
}
