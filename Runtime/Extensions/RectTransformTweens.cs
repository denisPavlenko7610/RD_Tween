using UnityEngine;

namespace RD_Tween.Runtime
{
	public static class RectTransformTweens
	{
		public static PropertyTween<Vector2> AnchorPositionTo(this RectTransform rt, Vector2 target, float duration)
		{
			return TweenFactory.To(
				rt,
				() => rt.anchoredPosition,
				v => rt.anchoredPosition = v,
				target,
				duration
			);
		}

		public static PropertyTween<Vector2> SizeDeltaTo(this RectTransform rt, Vector2 target, float duration)
		{
			return TweenFactory.To(
				rt,
				() => rt.sizeDelta,
				v => rt.sizeDelta = v,
				target,
				duration
			);
		}

		public static PropertyTween<Vector2> AnchorMinTo(this RectTransform rt, Vector2 target, float duration)
		{
			return TweenFactory.To(
				rt,
				() => rt.anchorMin,
				v => rt.anchorMin = v,
				target,
				duration
			);
		}

		public static PropertyTween<Vector2> AnchorMaxTo(this RectTransform rt, Vector2 target, float duration)
		{
			return TweenFactory.To(
				rt,
				() => rt.anchorMax,
				v => rt.anchorMax = v,
				target,
				duration
			);
		}

		public static PropertyTween<Vector2> PivotTo(this RectTransform rt, Vector2 target, float duration)
		{
			return TweenFactory.To(
				rt,
				() => rt.pivot,
				v => rt.pivot = v,
				target,
				duration
			);
		}
	}
}
