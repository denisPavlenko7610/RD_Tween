using System;
using UnityEngine;

namespace RD_Tween.Runtime
{
	public static class Tween
	{
		public static PropertyTween<float> To(
			System.Object target,
			Func<float> getter,
			Action<float> setter,
			float endValue,
			float duration,
			bool autoPlay = true)
		{
			return TweenFactory.To(target, getter, setter, endValue, duration, autoPlay);
		}

		public static PropertyTween<Vector3> To(
			System.Object target,
			Func<Vector3> getter,
			Action<Vector3> setter,
			Vector3 endValue,
			float duration,
			bool autoPlay = true)
		{
			return TweenFactory.To(target, getter, setter, endValue, duration, autoPlay);
		}

		public static PropertyTween<Vector2> To(
			System.Object target,
			Func<Vector2> getter,
			Action<Vector2> setter,
			Vector2 endValue,
			float duration,
			bool autoPlay = true)
		{
			return TweenFactory.To(target, getter, setter, endValue, duration, autoPlay);
		}

		public static PropertyTween<Color> To(
			System.Object target,
			Func<Color> getter,
			Action<Color> setter,
			Color endValue,
			float duration,
			bool autoPlay = true)
		{
			return TweenFactory.To(target, getter, setter, endValue, duration, autoPlay);
		}

		public static PropertyTween<Quaternion> To(
			System.Object target,
			Func<Quaternion> getter,
			Action<Quaternion> setter,
			Quaternion endValue,
			float duration,
			bool autoPlay = true)
		{
			return TweenFactory.To(target, getter, setter, endValue, duration, autoPlay);
		}

		public static TweenSequence Sequence(bool autoPlay = true)
		{
			return TweenSequence.Rent(autoPlay);
		}

		public static void Kill(System.Object target, bool complete = false)
		{
			TweenManager.Kill(target, complete);
		}

		public static void KillAll(bool complete = false)
		{
			TweenManager.KillAll(complete);
		}

		public static void KillById(object id, bool complete = false)
		{
			TweenManager.KillId(id, complete);
		}

		public static void PauseAll()
		{
			TweenManager.PauseAll();
		}

		public static void ResumeAll()
		{
			TweenManager.ResumeAll();
		}

		public static void CompleteAll()
		{
			TweenManager.KillAll(true);
		}

		public static bool IsTweening(System.Object target)
		{
			return TweenManager.IsTweening(target);
		}

		public static bool IsTweeningId(object id)
		{
			return TweenManager.IsTweeningId(id);
		}

		public static int ActiveCount()
		{
			return TweenManager.ActiveCount();
		}

		public static void GotoAll(float normalizedTime, bool play = false)
		{
			TweenManager.GotoAll(normalizedTime, play);
		}
	}
}
