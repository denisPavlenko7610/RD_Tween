using UnityEngine;

namespace RD_Tween.Runtime
{
	public static class TransformTweens
	{
		public static PropertyTween<Vector3> MoveTo(this Transform transform, Vector3 target, float duration) =>
			TweenFactory.To(
				transform,
				() => transform.position,
				vector3 => transform.position = vector3,
				target,
				duration
			);

		public static PropertyTween<Vector3> LocalMoveTo(this Transform transform, Vector3 target, float duration) =>
			TweenFactory.To(
				transform,
				() => transform.localPosition,
				vector3 => transform.localPosition = vector3,
				target,
				duration
			);

		public static PropertyTween<Vector3> ScaleTo(this Transform transform, Vector3 target, float duration) =>
			TweenFactory.To(
				transform,
				() => transform.localScale,
				vector3 => transform.localScale = vector3,
				target,
				duration
			);

		public static PropertyTween<Vector3> RotateTo(this Transform transform, Vector3 euler, float duration) =>
			TweenFactory.To(
				transform,
				() => transform.eulerAngles,
				vector3 => transform.eulerAngles = vector3,
				euler,
				duration
			);

		public static TweenActionCore RotateByX(this Transform transform, float angle, float duration) =>
			transform.RotateBy(Vector3.right, angle, duration);

		public static TweenActionCore RotateByY(this Transform transform, float angle, float duration) =>
			transform.RotateBy(Vector3.up, angle, duration);

		public static TweenActionCore RotateByZ(this Transform transform, float angle, float duration) =>
			transform.RotateBy(Vector3.forward, angle, duration);

		public static TweenActionCore RotateBy(this Transform transform, Vector3 axis, float angle, float duration)
		{
			var tweenActionCore = new TweenActionCore();
			tweenActionCore.ResetCore(transform, autoPlay: true);

			Quaternion start = default;

			tweenActionCore.AddStep(
				duration,
				t =>
				{
					float a = Mathf.LerpUnclamped(0, angle, t);
					transform.rotation = start * Quaternion.AngleAxis(a, axis);
				},
				null,
				() => start = transform.rotation
			);

			tweenActionCore.Play();
			return tweenActionCore;
		}

		public static TweenActionCore JumpTo(this Transform transform, Vector3 target, float duration, float height)
		{
			var tweenActionCore = new TweenActionCore();
			tweenActionCore.ResetCore(transform, autoPlay: true);

			Vector3 start = default;

			tweenActionCore.AddStep(
				duration,
				t =>
				{
					float y = Mathf.Sin(t * Mathf.PI) * height;
					transform.position = new Vector3(
						Mathf.LerpUnclamped(start.x, target.x, t),
						Mathf.LerpUnclamped(start.y, target.y, t) + y,
						Mathf.LerpUnclamped(start.z, target.z, t)
					);
				},
				null,
				() => start = transform.position
			);

			tweenActionCore.Play();
			return tweenActionCore;
		}

		public static PropertyTween<Vector3> ManualTween(this Transform transform) => TweenFactory.To(
			transform,
			() => transform.position,
			vector3 => transform.position = vector3,
			transform.position,
			0.0001f,
			autoPlay: false
		);
	}
}
