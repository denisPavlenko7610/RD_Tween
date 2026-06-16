using System;
using System.Collections;
using UnityEngine;

namespace RD_Tween.Runtime
{
	public static class TweenUtility
	{
		public static TweenActionCore DelayedCall(float delay, Action callback)
		{
			TweenActionCore tween = new TweenActionCore();
			tween.ResetCore(null, true);
			tween.SetDelay(delay);
			tween.AppendCallback(callback);
			tween.Play();
			return tween;
		}

		public static IEnumerator WaitForCompletion(this TweenActionCore tween)
		{
			while (tween != null && !tween.killed && tween.paused == false)
				yield return null;
		}

		public static IEnumerator WaitForKill(this TweenActionCore tween)
		{
			while (tween != null && !tween.killed)
				yield return null;
		}

		public static IEnumerator WaitForElapsedLoops(this TweenActionCore tween, int loops)
		{
			int lastLoops = 0;
			while (tween != null && !tween.killed && lastLoops < loops)
			{
				int currentLoops = tween.LoopsDone;
				if (currentLoops > lastLoops)
					lastLoops = currentLoops;
				yield return null;
			}
		}

		public static IEnumerator WaitForPosition(this TweenActionCore tween, float positionRatio)
		{
			while (tween != null && !tween.killed)
			{
				if (tween.delayElapsed / (tween.Duration + tween.Delay) >= positionRatio)
					yield break;
				yield return null;
			}
		}

		public static TweenSequence AppendCoroutine(this TweenSequence sequence, MonoBehaviour runner, Func<IEnumerator> coroutineFactory)
		{
			if (sequence == null || runner == null || coroutineFactory == null)
				return sequence;

			runner.StartCoroutine(WrapCoroutine(sequence, coroutineFactory));
			return sequence;
		}

		private static IEnumerator WrapCoroutine(TweenSequence sequence, Func<IEnumerator> coroutineFactory)
		{
			yield return coroutineFactory();
		}
	}
}
