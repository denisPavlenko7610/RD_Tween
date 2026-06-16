using System;
using UnityEngine;

namespace RD_Tween.Runtime
{
	public static class TransformTweens
	{
		// ---- Move ----
		public static PropertyTween<Vector3> MoveTo(this Transform transform, Vector3 target, float duration)
		{
			return TweenFactory.To(
				transform,
				() => transform.position,
				vector3 => transform.position = vector3,
				target,
				duration
			);
		}

		public static PropertyTween<Vector3> LocalMoveTo(this Transform transform, Vector3 target, float duration)
		{
			return TweenFactory.To(
				transform,
				() => transform.localPosition,
				vector3 => transform.localPosition = vector3,
				target,
				duration
			);
		}

		// ---- Scale ----
		public static PropertyTween<Vector3> ScaleTo(this Transform transform, Vector3 target, float duration)
		{
			return TweenFactory.To(
				transform,
				() => transform.localScale,
				vector3 => transform.localScale = vector3,
				target,
				duration
			);
		}

		// ---- Rotate ----
		public static PropertyTween<Vector3> RotateTo(this Transform transform, Vector3 euler, float duration)
		{
			return TweenFactory.To(
				transform,
				() => transform.eulerAngles,
				vector3 => transform.eulerAngles = vector3,
				euler,
				duration
			);
		}

		public static PropertyTween<Quaternion> LocalRotateTo(this Transform transform, Quaternion target, float duration)
		{
			return TweenFactory.To(
				transform,
				() => transform.localRotation,
				q => transform.localRotation = q,
				target,
				duration
			);
		}

		// ---- Rotate By ----
		public static TweenActionCore RotateByX(this Transform transform, float angle, float duration)
		{
			return transform.RotateBy(Vector3.right, angle, duration);
		}

		public static TweenActionCore RotateByY(this Transform transform, float angle, float duration)
		{
			return transform.RotateBy(Vector3.up, angle, duration);
		}

		public static TweenActionCore RotateByZ(this Transform transform, float angle, float duration)
		{
			return transform.RotateBy(Vector3.forward, angle, duration);
		}

		public static TweenActionCore RotateBy(this Transform transform, Vector3 axis, float angle, float duration)
		{
			TweenActionCore core = new TweenActionCore();
			core.ResetCore(transform, true);

			Quaternion start = default;

			core.AddStep(
				duration,
				t =>
				{
					float a = Mathf.LerpUnclamped(0, angle, t);
					transform.rotation = start * Quaternion.AngleAxis(a, axis);
				},
				null,
				() => start = transform.rotation
			);

			core.Play();
			return core;
		}

		// ---- Jump ----
		public static TweenActionCore JumpTo(this Transform transform, Vector3 target, float duration, float height)
		{
			TweenActionCore core = new TweenActionCore();
			core.ResetCore(transform, true);

			Vector3 startPos = default;
			Vector3 startLocal = default;
			bool useLocal = false;

			core.AddStep(
				duration,
				t =>
				{
					float y = Mathf.Sin(t * Mathf.PI) * height;
					Vector3 pos = useLocal ? startLocal : startPos;
					transform.position = new Vector3(
						Mathf.LerpUnclamped(pos.x, target.x, t),
						Mathf.LerpUnclamped(pos.y, target.y, t) + y,
						Mathf.LerpUnclamped(pos.z, target.z, t)
					);
				},
				null,
				() =>
				{
					startPos = transform.position;
					startLocal = transform.localPosition;
				}
			);

			core.Play();
			return core;
		}

		// ---- Punch Position ----
		public static TweenActionCore PunchPosition(this Transform transform, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1f)
		{
			TweenActionCore core = new TweenActionCore();
			core.ResetCore(transform, true);

			Vector3 start = default;

			core.AddStep(
				duration,
				t =>
				{
					float p = CurvesType.Punch(1f, t);
					float vibratoVal = 1f;
					for (int i = 0; i < vibrato; i++)
						vibratoVal *= elasticity;
					Vector3 offset = punch * (p * vibratoVal);
					transform.position = start + offset;
				},
				null,
				() => start = transform.position
			);

			core.Play();
			return core;
		}

		// ---- Punch Scale ----
		public static TweenActionCore PunchScale(this Transform transform, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1f)
		{
			TweenActionCore core = new TweenActionCore();
			core.ResetCore(transform, true);

			Vector3 start = default;

			core.AddStep(
				duration,
				t =>
				{
					float p = CurvesType.Punch(1f, t);
					transform.localScale = start + punch * p;
				},
				null,
				() => start = transform.localScale
			);

			core.Play();
			return core;
		}

		// ---- Punch Rotation ----
		public static TweenActionCore PunchRotation(this Transform transform, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1f)
		{
			TweenActionCore core = new TweenActionCore();
			core.ResetCore(transform, true);

			Vector3 start = default;

			core.AddStep(
				duration,
				t =>
				{
					float p = CurvesType.Punch(1f, t);
					transform.eulerAngles = start + punch * p;
				},
				null,
				() => start = transform.eulerAngles
			);

			core.Play();
			return core;
		}

		// ---- Shake Position ----
		public static TweenActionCore ShakePosition(this Transform transform, float strength, float duration, int vibrato = 10, float randomness = 90f)
		{
			TweenActionCore core = new TweenActionCore();
			core.ResetCore(transform, true);

			Vector3 start = default;

			core.AddStep(
				duration,
				t =>
				{
					float p = CurvesType.Punch(strength, t);
					float randX = UnityEngine.Random.Range(-1f, 1f) * randomness;
					float randY = UnityEngine.Random.Range(-1f, 1f) * randomness;
					float randZ = UnityEngine.Random.Range(-1f, 1f) * randomness;
					Vector3 shakeOffset = new Vector3(
						p * (1f + randX * 0.01f),
						p * (1f + randY * 0.01f),
						p * (1f + randZ * 0.01f)
					);
					transform.position = start + shakeOffset;
				},
				null,
				() => start = transform.position
			);

			core.Play();
			return core;
		}

		// ---- Shake Scale ----
		public static TweenActionCore ShakeScale(this Transform transform, float strength, float duration, int vibrato = 10, float randomness = 90f)
		{
			TweenActionCore core = new TweenActionCore();
			core.ResetCore(transform, true);

			Vector3 start = default;

			core.AddStep(
				duration,
				t =>
				{
					float p = CurvesType.Punch(strength, t);
					float randX = UnityEngine.Random.Range(-1f, 1f) * randomness;
					float randY = UnityEngine.Random.Range(-1f, 1f) * randomness;
					float randZ = UnityEngine.Random.Range(-1f, 1f) * randomness;
					Vector3 offset = new Vector3(
						p * (1f + randX * 0.01f),
						p * (1f + randY * 0.01f),
						p * (1f + randZ * 0.01f)
					);
					transform.localScale = start + offset;
				},
				null,
				() => start = transform.localScale
			);

			core.Play();
			return core;
		}

		// ---- Utility ----
		public static PropertyTween<Vector3> ManualTween(this Transform transform)
		{
			return TweenFactory.To(
				transform,
				() => transform.position,
				vector3 => transform.position = vector3,
				transform.position,
				0.0001f,
				false
			);
		}
	}
}
