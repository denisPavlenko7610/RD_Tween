using System;
using UnityEngine;

namespace RD_Tween.Runtime
{
	public static class TweenFactory
	{
		public static PropertyTween<float> To(
			UnityEngine.Object target,
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
				add: (a,b)=>a+b,
				sub: (a,b)=>a-b,
				mul: (a,f)=>a*f
			);
		}

		public static PropertyTween<Vector3> To(
			UnityEngine.Object target,
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
				add: (a,b)=>a+b,
				sub: (a,b)=>a-b,
				mul: (a,f)=>a*f
			);
		}
	}
}
