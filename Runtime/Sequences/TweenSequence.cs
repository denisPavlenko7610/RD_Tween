using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RD_Tween.Runtime
{
    public sealed class TweenSequence : TweenActionCore
    {
        private sealed class Item
        {
            public IControllableTween Tween;
            public float Start;
            public float End;

            public bool IsCallback;
            public Action Callback;

            public bool FiredForward;
            public bool FiredBackward;
        }

        // -------- Pool --------
        private const int MaxPool = 64;
        private static readonly Stack<TweenSequence> Pool = new();
        private bool _inPool;

        public static TweenSequence Rent(bool autoPlay = true)
        {
            TweenSequence sequence = Pool.Count > 0 ? Pool.Pop() : new TweenSequence();
            sequence._inPool = false;
            sequence.ResetCore(null, autoPlay);
            sequence._items.Clear();
            sequence._duration = 0f;
            sequence._lastAppendStart = 0f;
            sequence._time = 0f;
            return sequence;
        }

        public override void Release()
        {
            if (!AutoKill && !killed)
            {
                Pause();
                return;
            }

            if (_inPool)
				return;
			
            _inPool = true;
            _items.Clear();

            if (Pool.Count < MaxPool)
                Pool.Push(this);
        }
        // -----------------------

        private readonly List<Item> _items = new();
        private float _duration;
        private float _lastAppendStart;
        private float _time;

        private TweenSequence() { }

        public override float Duration => _duration;

        // ---- Timeline building ----
        public TweenSequence Append(IControllableTween t)
        {
            if (t == null) 
				return this;

            float start = _duration;
            _lastAppendStart = start;

            _items.Add(new Item
            {
                Tween = t,
                Start = start,
                End = start + t.Duration
            });

            _duration = Mathf.Max(_duration, start + t.Duration);
            Play();
            return this;
        }

        public TweenSequence Prepend(IControllableTween t)
        {
            if (t == null)
				return this;

            float shift = t.Duration;
            foreach (var item in _items) {
				item.Start += shift;
				item.End += shift;
			}

            _items.Insert(0, new Item
            {
                Tween = t,
                Start = 0f,
                End = shift
            });

            _duration += shift;
            _lastAppendStart += shift;

            Play();
            return this;
        }

        public TweenSequence Join(IControllableTween t)
        {
            if (t == null)
				return this;

            float start = _lastAppendStart;

            _items.Add(new Item
            {
                Tween = t,
                Start = start,
                End = start + t.Duration
            });

            _duration = Mathf.Max(_duration, start + t.Duration);
            Play();
            return this;
        }

        public TweenSequence Insert(float atTime, IControllableTween t)
        {
            if (t == null)
				return this;
			
            atTime = Mathf.Max(0f, atTime);

            _items.Add(new Item
            {
                Tween = t,
                Start = atTime,
                End = atTime + t.Duration
            });

            _duration = Mathf.Max(_duration, atTime + t.Duration);
            Play();
            return this;
        }

        public TweenSequence AppendInterval(float duration)
        {
            duration = Mathf.Max(0f, duration);
            _duration += duration;
            _lastAppendStart = _duration - duration;
            Play();
            return this;
        }

        public TweenSequence PrependInterval(float duration)
        {
            duration = Mathf.Max(0f, duration);
            foreach (var item in _items) {
				item.Start += duration;
				item.End += duration;
			}
            _duration += duration;
            _lastAppendStart += duration;
            Play();
            return this;
        }

        public TweenSequence AppendCallback(Action cb)
        {
            if (cb == null) 
				return this;

            float time = _duration;

            _items.Add(new Item
            {
                IsCallback = true,
                Callback = cb,
                Start = time,
                End = time
            });

            Play();
            return this;
        }

        // ---- Scrub overriden for timeline ----
        public override void Rewind(bool play = false)
        {
            base.Rewind(play: false);
            _time = 0f;
            ResetCallbackFlags();
            EvaluateScrub(_time);

            if (!play)
				Pause();
            else 
				Resume();
        }

        public override void Goto(float time, bool play = false)
        {
            time = Mathf.Clamp(time, 0f, _duration);
            base.Goto(0f, false);
            _time = time;
            EvaluateScrub(_time);

            if (!play)
				Pause();
            else
				Resume();
        }

        public override void Complete(bool withCallbacks = true)
        {
            _time = _duration;
            EvaluateScrub(_time);
            if (withCallbacks)
				base.Complete(true);
            else 
				base.Complete(false);
        }

        public override bool UpdateTween(float deltaTime)
        {
            if (!base.UpdateTween(0f))
				return false; 
			
            if (killed)
				return false;
			
            if (paused) 
				return true;

            float dt = deltaTime * Speed;

            if (delayElapsed < Delay)
            {
                delayElapsed += dt;
                if (delayElapsed < Delay)
					return true;
            }

            float prevTime = _time;
            _time += dt * direction;

            if (direction > 0)
            {
                if (_time >= _duration)
                {
                    _time = _duration;
                    EvaluateCross(prevTime, _time, true);

                    if (HandleLoopBoundary(true))
                        return true;

                    return Finish(true);
                }
                EvaluateCross(prevTime, _time, true);
            }
            else
            {
                if (_time <= 0f)
                {
                    _time = 0f;
                    EvaluateCross(prevTime, _time, false);

                    if (HandleLoopBoundary(false))
                        return true;

                    return Finish(false);
                }
                EvaluateCross(prevTime, _time, false);
            }

            return true;
        }

        private void EvaluateScrub(float time)
		{
			foreach (Item item in _items) {
				if (item.IsCallback) 
					continue;

				float local = Mathf.Clamp(time - item.Start, 0f, item.Tween.Duration);
				item.Tween.Goto(local, play: false);
			}
		}

        private void EvaluateCross(float prev, float now, bool forward)
		{
            EvaluateScrub(now);
			foreach (Item item in _items) {
				if (!item.IsCallback)
					continue;

				if (forward)
				{
					if (!item.FiredForward && prev < item.Start && now >= item.Start)
					{
						item.FiredForward = true;
						item.Callback?.Invoke();
					}
				}
				else
				{
					if (!item.FiredBackward && prev > item.Start && now <= item.Start)
					{
						item.FiredBackward = true;
						item.Callback?.Invoke();
					}
				}
			}
		}

        private bool HandleLoopBoundary(bool forwardEnded)
        {
            if (Loops == 1 || (Loops != -1 && LoopsDone >= Loops - 1))
                return false;

            LoopsDone++;

            ResetCallbackFlags();

            if (CurrentLoopType == LoopType.Yoyo)
            {
                direction *= -1;
                _time = forwardEnded ? _duration : 0f;
                return true;
            }

            foreach (Item item in _items.Where(item => !item.IsCallback))
				item.Tween.Rewind(false);

            direction = 1;
            _time = 0f;
            return true;
        }

        private void ResetCallbackFlags()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].FiredForward = false;
                _items[i].FiredBackward = false;
            }
        }
    }
}
