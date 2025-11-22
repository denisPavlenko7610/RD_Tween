using System;

namespace RD_Tween.Runtime
{
	public sealed class Step
	{
		public float Duration;
		public Func<float, float> Ease;
		public Action<float> Apply;
		public Action OnStart;
		public bool Started;

		public Action<object> ConfigFrom;
		public Action<bool> ConfigRelative;
	}
}
