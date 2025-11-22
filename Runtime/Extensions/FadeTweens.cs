using UnityEngine;
using UnityEngine.UI;

namespace RD_Tween.Runtime
{
	public static class FadeTweens
	{
		public static PropertyTween<float> FadeTo(this CanvasGroup cg, float alpha, float dur)
			=> TweenFactory.To(cg, () => cg.alpha, v => cg.alpha = v, alpha, dur);

		public static PropertyTween<float> FadeTo(this SpriteRenderer sr, float alpha, float dur)
			=> TweenFactory.To(sr, () => sr.color.a,
				v => { var c = sr.color; c.a = v; sr.color = c; },
				alpha, dur);

		public static PropertyTween<float> FadeTo(this Graphic g, float alpha, float dur)
			=> TweenFactory.To(g, () => g.color.a,
				v => { var c = g.color; c.a = v; g.color = c; },
				alpha, dur);
	}
}
