using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RD_Tween.Runtime
{
	public class TweenActionCore : IControllableTween, IPooledTween
	{
		internal enum FromMode
		{
			None,
			Absolute,
			Relative,
			Current
		}

		internal readonly struct FromInstruction
		{
			public readonly FromMode Mode;
			public readonly object Value;

			public FromInstruction(FromMode mode, object value = null)
			{
				Mode = mode;
				Value = value;
			}
		}

		protected readonly List<Step> Steps = new();

		public UnityEngine.Object Target { get; private set; }
		public object Id { get; private set; }

		public UpdateType UpdateType { get; private set; } = UpdateType.Normal;
		public bool IsUnscaled { get; private set; }

		public float Delay { get; private set; }
		public float delayElapsed { get; set; }

		public float Speed { get; private set; } = 1f;
		public int direction { get; set; } = 1;

		public int Loops { get; private set; } = 1;
		protected int LoopsDone { get; set; }
		public LoopType CurrentLoopType { get; private set; } = LoopType.Restart;

		private int _currentIndex;
		private float _elapsed;

		private bool _autoPlay;
		public bool AutoKill { get; private set; } = true;

		private bool _isPlaying;
		public bool paused { get; private set; }
		public bool killed { get; private set; }

		private Action _onComplete, _onPlay, _onPause, _onKill;

		public virtual void Release() { }

		public void ResetCore(UnityEngine.Object target, bool autoPlay)
		{
			Target = target;
			Id = null;
			Steps.Clear();

			UpdateType = UpdateType.Normal;
			IsUnscaled = false;

			Delay = 0f;
			delayElapsed = 0f;

			Speed = 1f;
			direction = 1;

			Loops = 1;
			LoopsDone = 0;
			CurrentLoopType = LoopType.Restart;

			_currentIndex = 0;
			_elapsed = 0f;

			_autoPlay = autoPlay;
			AutoKill = true;

			_isPlaying = false;
			paused = false;
			killed = false;

			_onComplete = _onPlay = _onPause = _onKill = null;
		}

		public bool IsDead() => Target == null;

		public virtual float Duration
		{
			get
			{
				return Steps.Sum(t => t.Duration);
			}
		}

		public TweenActionCore AddStep(
			float duration,
			Action<float> apply,
			Func<float, float> ease = null,
			Action onStart = null,
			Action<object> configFrom = null,
			Action<bool> configRelative = null
		)
		{
			Steps.Add(
				new Step
				{
					Duration = Mathf.Max(0.0001f, duration),
					Apply = apply,
					Ease = ease ?? CurvesType.Linear,
					OnStart = onStart,
					Started = false,
					ConfigFrom = configFrom,
					ConfigRelative = configRelative
				}
			);

			if (_autoPlay)
				Autoplay();
			
			return this;
		}

		private void Autoplay()
		{
			if (killed)
				return;
			
			if (_isPlaying)
				return;

			_isPlaying = true;
			paused = false;
			TweenManager.Instance.AddTween(this);
			_onPlay?.Invoke();
		}

		public void Play() => Autoplay();

		public TweenActionCore SetUpdate(UpdateType type, bool isUnscaled = false)
		{
			UpdateType oldType = UpdateType;
			bool oldUnscaled = IsUnscaled;

			UpdateType = type;
			IsUnscaled = isUnscaled;

			if (_isPlaying)
				TweenManager.Instance.ChangeUpdateType(this, oldType, oldUnscaled);

			return this;
		}

		public TweenActionCore SetDelay(float delay)
		{
			Delay = Mathf.Max(0f, delay);
			return this;
		}

		public TweenActionCore SetSpeed(float speed)
		{
			Speed = Mathf.Max(0f, speed);
			return this;
		}

		public TweenActionCore SetLoops(int loops, LoopType type = LoopType.Restart)
		{
			Loops = loops;
			CurrentLoopType = type;
			return this;
		}

		public TweenActionCore SetAutoKill(bool autoKill = true)
		{
			AutoKill = autoKill;
			return this;
		}

		public TweenActionCore SetId(object id)
		{
			Id = id;
			return this;
		}

		public TweenActionCore SetEase(Func<float, float> ease)
		{
			if (Steps.Count == 0)
				return this;

			Steps[^1].Ease = ease ?? CurvesType.Linear;
			return this;
		}

		public TweenActionCore SetRelative(bool relative = true)
		{
			if (Steps.Count == 0)
				return this;

			Steps[^1].ConfigRelative?.Invoke(relative);
			return this;
		}

		public TweenActionCore From(Vector3 value)
		{
			if (Steps.Count == 0)
				return this;

			Steps[^1].ConfigFrom?.Invoke(new FromInstruction(FromMode.Absolute, value));
			return this;
		}

		public TweenActionCore From(float value)
		{
			if (Steps.Count == 0)
				return this;

			Steps[^1].ConfigFrom?.Invoke(new FromInstruction(FromMode.Absolute, value));
			return this;
		}

		public TweenActionCore FromRelative(Vector3 offset)
		{
			if (Steps.Count == 0)
				return this;

			Steps[^1].ConfigFrom?.Invoke(new FromInstruction(FromMode.Relative, offset));
			return this;
		}

		public TweenActionCore FromRelative(float offset)
		{
			if (Steps.Count == 0)
				return this;

			Steps[^1].ConfigFrom?.Invoke(new FromInstruction(FromMode.Relative, offset));
			return this;
		}

		public TweenActionCore FromCurrent()
		{
			if (Steps.Count == 0)
				return this;

			Steps[^1].ConfigFrom?.Invoke(new FromInstruction(FromMode.Current));
			return this;
		}

		public TweenActionCore AppendInterval(float duration) => AddStep(duration, _ => { });
		public TweenActionCore AppendCallback(Action cb) => AddStep(0.0001f, _ => { }, null, cb);

		// ---- Events ----
		public TweenActionCore OnComplete(Action a)
		{
			_onComplete = a;
			return this;
		}

		public TweenActionCore OnPlay(Action a)
		{
			_onPlay = a;
			return this;
		}

		public TweenActionCore OnPause(Action a)
		{
			_onPause = a;
			return this;
		}

		public TweenActionCore OnKill(Action a)
		{
			_onKill = a;
			return this;
		}

		// ---- Controls ----
		public TweenActionCore Pause()
		{
			if (!paused) {
				paused = true;
				_onPause?.Invoke();
			}
			return this;
		}

		public TweenActionCore Resume()
		{
			if (paused) {
				paused = false;
				Autoplay();
			}
			return this;
		}

		public TweenActionCore Kill()
		{
			if (killed)
				return this;

			killed = true;
			_onKill?.Invoke();
			return this;
		}

		public TweenActionCore PlayForward()
		{
			direction = 1;
			paused = false;
			Autoplay();
			return this;
		}

		public TweenActionCore PlayBackwards()
		{
			if (Steps.Count == 0)
				return this;

			if (_currentIndex >= Steps.Count) {
				_currentIndex = Steps.Count - 1;
				_elapsed = Steps[_currentIndex].Duration;
			}
			else if (_elapsed <= 0f) {
				_elapsed = Steps[_currentIndex].Duration;
			}

			direction = -1;
			paused = false;
			Autoplay();
			return this;
		}

		public TweenActionCore PlayFromEnd()
		{
			Goto(Duration, false);
			_currentIndex = Steps.Count - 1;
			_elapsed = Steps[_currentIndex].Duration;
			return PlayBackwards();
		}

		public TweenActionCore Restart(bool play = true)
		{
			Rewind(false);
			if (play)
				PlayForward();
			
			return this;
		}

		// ---- Scrub ----
		public virtual void Rewind(bool play = false)
		{
			if (Steps.Count == 0)
				return;

			ResetSteps();
			_currentIndex = 0;
			_elapsed = 0f;
			LoopsDone = 0;
			direction = 1;
			killed = false;
			delayElapsed = 0f;

			PrepareStepStart(0);
			ApplyStepAt(0, 0f);

			if (!play)
				Pause();
			else
				Resume();
		}

		public virtual void Goto(float time, bool play = false)
		{
			time = Mathf.Clamp(time, 0f, Duration);

			ResetSteps();
			_currentIndex = 0;
			_elapsed = 0f;
			LoopsDone = 0;
			direction = 1;
			killed = false;
			delayElapsed = Delay;

			float remaining = time;

			for (int i = 0; i < Steps.Count; i++) {
				PrepareStepStart(i);
				float d = Steps[i].Duration;

				if (remaining >= d) {
					ApplyStepAt(i, 1f);
					remaining -= d;
					_currentIndex = i + 1;
				}
				else {
					float localT = remaining / d;
					ApplyStepAt(i, localT);
					_currentIndex = i;
					_elapsed = remaining;
					break;
				}
			}

			if (!play)
				Pause();
			else
				Resume();
		}

		public void GotoNormalized(float t01, bool play = false) => Goto(Duration * Mathf.Clamp01(t01), play);

		public virtual void Complete(bool withCallbacks = true)
		{
			Goto(Duration, false);
			if (withCallbacks)
				_onComplete?.Invoke();

			if (AutoKill) {
				killed = true;
				Release();
			}
			else {
				paused = true;
				_isPlaying = false;
			}
		}

		// ---- Update ----
		public virtual bool UpdateTween(float deltaTime)
		{
			if (killed)
				return Finish(false);

			if (paused)
				return true;

			if (Steps.Count == 0)
				return Finish(false);

			float dt = deltaTime * Speed;

			if (delayElapsed < Delay) {
				delayElapsed += dt;
				if (delayElapsed < Delay)
					return true;
			}

			return direction >= 0 ? UpdateForward(dt) : UpdateBackward(dt);
		}

		private bool UpdateForward(float dt)
		{
			if (_currentIndex >= Steps.Count)
				return HandleLoopOrFinish(true);

			PrepareStepStart(_currentIndex);

			_elapsed += dt;
			float d = Steps[_currentIndex].Duration;
			float t01 = Mathf.Clamp01(_elapsed / d);

			ApplyStepAt(_currentIndex, t01);

			if (_elapsed < d)
				return true;

			_elapsed = 0f;
			_currentIndex++;
			return true;
		}

		private bool UpdateBackward(float dt)
		{
			if (_currentIndex < 0)
				return HandleLoopOrFinish(false);

			PrepareStepStart(_currentIndex);

			_elapsed -= dt;
			float d = Steps[_currentIndex].Duration;
			float t01 = Mathf.Clamp01(_elapsed / d);

			ApplyStepAt(_currentIndex, t01);

			if (_elapsed > 0f)
				return true;

			_currentIndex--;
			if (_currentIndex >= 0)
				_elapsed = Steps[_currentIndex].Duration;

			return true;
		}

		private bool HandleLoopOrFinish(bool forwardEnded)
		{
			if (Loops == 1 || (Loops != -1 && LoopsDone >= Loops - 1))
				return Finish(forwardEnded);

			LoopsDone++;

			switch (CurrentLoopType) {
				case LoopType.Restart:
				case LoopType.Incremental:
					ResetSteps();
					_currentIndex = 0;
					_elapsed = 0f;
					direction = 1;
					return true;

				case LoopType.Yoyo:
					if (direction == 1) {
						direction = -1;
						_currentIndex = Steps.Count - 1;
						PrepareStepStart(_currentIndex);
						_elapsed = Steps[_currentIndex].Duration;
					}
					else {
						direction = 1;
						ResetSteps();
						_currentIndex = 0;
						_elapsed = 0f;
					}
					return true;
			}

			return Finish(forwardEnded);
		}

		protected bool Finish(bool completedForward)
		{
			if (completedForward)
				_onComplete?.Invoke();
			
			Release();
			return false;
		}

		protected void ResetSteps()
		{
			foreach (Step step in Steps)
				step.Started = false;
		}

		protected void PrepareStepStart(int index)
		{
			Step step = Steps[index];
			if (step.Started)
				return;

			step.Started = true;
			step.OnStart?.Invoke();
		}

		protected void ApplyStepAt(int index, float t01)
		{
			Step step = Steps[index];
			float t = step.Ease(t01);
			step.Apply?.Invoke(t);
		}
	}
}
