using UnityEngine;
using UnityEngine.UI;

namespace RD_Tween.Runtime
{
	public static class GraphicTweens
	{
		public static PropertyTween<Color> ColorTo(this Graphic graphic, Color target, float duration)
		{
			return TweenFactory.To(
				graphic,
				() => graphic.color,
				v => graphic.color = v,
				target,
				duration
			);
		}

		public static PropertyTween<Color> ColorTo(this SpriteRenderer sr, Color target, float duration)
		{
			return TweenFactory.To(
				sr,
				() => sr.color,
				v => sr.color = v,
				target,
				duration
			);
		}

		public static PropertyTween<float> FillAmountTo(this Image image, float target, float duration)
		{
			return TweenFactory.To(
				image,
				() => image.fillAmount,
				v => image.fillAmount = v,
				target,
				duration
			);
		}

		public static PropertyTween<Color> ColorTo(this Camera camera, Color target, float duration)
		{
			return TweenFactory.To(
				camera,
				() => camera.backgroundColor,
				v => camera.backgroundColor = v,
				target,
				duration
			);
		}

		// ---- Fade ----
		public static PropertyTween<float> FadeTo(this Graphic graphic, float target, float duration)
		{
			return TweenFactory.To(
				graphic,
				() => graphic.color.a,
				v =>
				{
					Color c = graphic.color;
					c.a = v;
					graphic.color = c;
				},
				target,
				duration
			);
		}

		public static PropertyTween<float> FadeTo(this CanvasGroup canvasGroup, float target, float duration)
		{
			return TweenFactory.To(
				canvasGroup,
				() => canvasGroup.alpha,
				v => canvasGroup.alpha = v,
				target,
				duration
			);
		}

		public static PropertyTween<float> FadeTo(this SpriteRenderer spriteRenderer, float target, float duration)
		{
			return TweenFactory.To(
				spriteRenderer,
				() => spriteRenderer.color.a,
				v =>
				{
					Color c = spriteRenderer.color;
					c.a = v;
					spriteRenderer.color = c;
				},
				target,
				duration
			);
		}
	}
}
