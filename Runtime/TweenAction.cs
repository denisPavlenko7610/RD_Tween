using System;
using System.Collections.Generic;
using UnityEngine;

namespace RD_Tween.Runtime
{
    public class TweenAction
    {
        private Transform _transform;
        private List<Action<float>> _actions = new();
        private List<float> _durations = new();
        private List<Func<float, float>> _easings = new();
        private float _speed = 1f;
        private int _currentActionIndex;
        private float _elapsed;
        private int _loopCount = 1;
        private Action _onComplete;
        private List<TweenAction> _joinedTweens = new();

        public TweenAction(Transform transform)
        {
            _transform = transform;
        }

        public bool IsDead()
        {
            return _transform == null;
        }

        public TweenAction MoveTo(Vector3 target, float duration)
        {
            Vector3 startValue = _transform.position;
            _actions.Add(t => _transform.position = Vector3.Lerp(startValue, target, t));
            _durations.Add(duration);
            _easings.Add(t => t); // Default to linear easing
            return this;
        }

        public TweenAction RotateTo(Vector3 target, float duration)
        {
            Vector3 startValue = _transform.eulerAngles;
            _actions.Add(t => _transform.eulerAngles = Vector3.Slerp(startValue, target, t));
            _durations.Add(duration);
            _easings.Add(t => t); // Default to linear easing
            return this;
        }
        
        public TweenAction RotateBy(Vector3 axis, float angle, float duration)
        {
            Quaternion startRotation = _transform.rotation; // Starting rotation

            _actions.Add(t =>
            {
                // Calculate the interpolated rotation based on t
                float currentAngle = Mathf.Lerp(0, angle, t); // t goes from 0 to 1 over the duration
                Quaternion rotationDelta = Quaternion.AngleAxis(currentAngle, axis);

                // Apply the rotation relative to the start rotation
                _transform.rotation = startRotation * rotationDelta;
            });

            _durations.Add(duration);
            _easings.Add(t => t); // Linear easing for constant speed
            return this;
        }

        public TweenAction ScaleTo(Vector3 target, float duration)
        {
            var startValue = _transform.localScale;
            _actions.Add(t => _transform.localScale = Vector3.Lerp(startValue, target, t));
            _durations.Add(duration);
            _easings.Add(t => t);
            return this;
        }
        
        public TweenAction JumpTo(Vector3 target, float duration, float jumpHeight)
        {
            var startValue = _transform.position;
            _actions.Add(t =>
            {
                // Calculate the parabolic jump
                float y = Mathf.Sin(t * Mathf.PI) * jumpHeight; // Bounce effect using sine
                _transform.position = new Vector3(
                    Mathf.Lerp(startValue.x, target.x, t),
                    Mathf.Lerp(startValue.y, target.y, t) + y,
                    Mathf.Lerp(startValue.z, target.z, t)
                );
            });
            _durations.Add(duration);
            _easings.Add(t => t); // Default to linear easing
            return this;
        }

        public TweenAction Loop(int count)
        {
            _loopCount = count;
            return this;
        }

        public TweenAction SetSpeed(float speed)
        {
            _speed = speed;
            return this;
        }

        public TweenAction SetEase(Func<float, float> ease = null)
        {
            ease ??= CurvesType.Linear;
        
            if (_easings.Count > 0)
                _easings[^1] = ease;
            return this;
        }

        public TweenAction OnComplete(Action onComplete)
        {
            _onComplete = onComplete;
            return this;
        }

        public TweenAction Join(TweenAction otherTween)
        {
            _joinedTweens.Add(otherTween);
            return this;
        }

        public void Play()
        {
            Tween.Instance.AddTween(this);
            foreach (var tween in _joinedTweens)
            {
                Tween.Instance.AddTween(tween); // Play joined tweens together
            }
        }

        public bool UpdateTween(float deltaTime)
        {
            if (_currentActionIndex >= _actions.Count)
                return false;

            _elapsed += deltaTime * _speed;
            float duration = _durations[_currentActionIndex];
            float t = Mathf.Clamp01(_elapsed / duration);
            t = _easings[_currentActionIndex](t);
            
            _actions[_currentActionIndex](t);
            
            if (!IsComplete(duration))
                return false;

            return _loopCount != 0;
        }

        private bool IsComplete(float duration)
        {
            if (_elapsed >= duration)
            {
                _elapsed = 0;
                _currentActionIndex++;
                
                if (_currentActionIndex >= _actions.Count)
                {
                    if (_loopCount > 0) _loopCount--;
                    if (_loopCount != 0) _currentActionIndex = 0;
                    else
                    {
                        _onComplete?.Invoke();
                        return false;
                    }
                }
            }

            return true;
        }
    }
}