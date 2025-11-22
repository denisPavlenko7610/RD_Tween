using System;
using System.Collections.Generic;

namespace RD_Tween.Runtime
{
    public sealed class PropertyTween<T> : TweenActionCore
    {
        // ---------- Pool ----------
        private const int MaxPool = 256;
        private static readonly Stack<PropertyTween<T>> Pool = new();
        private bool _inPool;

        public static PropertyTween<T> Rent(
            UnityEngine.Object target,
            bool autoPlay,
            Func<T> getter,
            Action<T> setter,
            T endValue,
            float duration,
            Func<T, T, float, T> lerpUnclamped,
            Func<T, T, T> add = null,
            Func<T, T, T> sub = null,
            Func<T, float, T> mul = null)
        {
            PropertyTween<T> tween = Pool.Count > 0 ? Pool.Pop() : new PropertyTween<T>();
            tween._inPool = false;

            tween.ResetCore(target, autoPlay);

            tween._getter = getter;
            tween._setter = setter;
            tween._baseEnd = endValue;
            tween._lerp = lerpUnclamped;
            tween._add = add;
            tween._sub = sub;
            tween._mul = mul;

            tween._relative = false;
            tween._from = new FromInstruction(FromMode.None);

            tween._deltaInit = false;
            tween._delta = default;

            tween.AddMainStep(duration);

            return tween;
        }

        public override void Release()
        {
            if (!AutoKill && !killed && Target != null)
            {
                Pause();
                return;
            }

            if (_inPool)
				return;
			
            _inPool = true;

            _getter = null;
            _setter = null;
            _lerp = null;
            _add = null;
            _sub = null;
            _mul = null;

            if (Pool.Count < MaxPool)
                Pool.Push(this);
        }
        // ------------------------------

        private Func<T> _getter;
        private Action<T> _setter;

        private T _baseEnd;
        private T _start;
        private T _end;

        private Func<T, T, float, T> _lerp;
        private Func<T, T, T> _add;
        private Func<T, T, T> _sub;
        private Func<T, float, T> _mul;

        private bool _relative;
        private FromInstruction _from;

        private bool _deltaInit;
        private T _delta;

        private PropertyTween() { }

        private void AddMainStep(float duration)
        {
            AddStep(
                duration,
                t => _setter(_lerp(_start, _end, t)),
                null,
                OnStartStep,
                obj => { if (obj is FromInstruction fi) _from = fi; },
                r => _relative = r
            );
        }

        private void OnStartStep()
        {
            T current = _getter();

            if (!_relative && !_deltaInit && _sub != null)
            {
                _delta = _sub(_baseEnd, current);
                _deltaInit = true;
            }

            float incFactor = (CurrentLoopType == LoopType.Incremental) ? (LoopsDone + 1) : 1f;

            T provisionalEnd;
            if (_relative)
            {
                if (_add == null || _mul == null)
                    provisionalEnd = _baseEnd;
                else
                    provisionalEnd = _add(current, _mul(_baseEnd, incFactor));
            }
            else
            {
                if (CurrentLoopType == LoopType.Incremental && _add != null && _mul != null && _deltaInit)
                    provisionalEnd = _add(_baseEnd, _mul(_delta, LoopsDone));
                else
                    provisionalEnd = _baseEnd;
            }

            // start
            _start = current;
            switch (_from.Mode)
            {
                case FromMode.Absolute:
                    _start = (T)_from.Value;
                    _setter(_start);
                    break;

                case FromMode.Relative:
                    if (_add != null)
                    {
                        _start = _add(provisionalEnd, (T)_from.Value);
                        _setter(_start);
                    }
                    break;

                case FromMode.Current:
                    _start = current;
                    break;
            }

            // end
            if (_relative)
            {
                if (_add != null && _mul != null)
                    _end = _add(_start, _mul(_baseEnd, incFactor));
                else
                    _end = _baseEnd;
            }
            else
            {
                if (CurrentLoopType == LoopType.Incremental && _add != null && _mul != null && _deltaInit)
                    _end = _add(_baseEnd, _mul(_delta, LoopsDone));
                else
                    _end = _baseEnd;
            }
        }
    }
}
