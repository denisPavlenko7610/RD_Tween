using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RD_Tween.Runtime
{
    public sealed class TweenManager
    {
        private static TweenManager _instance;
        public static TweenManager Instance => _instance ??= new TweenManager();

        private readonly List<TweenActionCore> _updateScaled = new();
        private readonly List<TweenActionCore> _updateUnscaled = new();
        private readonly List<TweenActionCore> _lateScaled = new();
        private readonly List<TweenActionCore> _lateUnscaled = new();
        private readonly List<TweenActionCore> _fixedScaled = new();
        private readonly List<TweenActionCore> _fixedUnscaled = new();

        private TweenRunner _runner;

        private TweenManager() => EnsureRunner();

        private void EnsureRunner()
        {
            if (_runner != null)
				return;

            var existing = Object.FindAnyObjectByType<TweenRunner>();
            if (existing != null)
            {
                _runner = existing;
                _runner.Init(this);
                return;
            }

            var go = new GameObject("[RD_TweenRunner]");
            Object.DontDestroyOnLoad(go);
            _runner = go.AddComponent<TweenRunner>();
            _runner.Init(this);
        }

        internal void AddTween(TweenActionCore tween)
        {
            if (tween == null)
				return;
			
            List<TweenActionCore> list = GetList(tween.UpdateType, tween.IsUnscaled);
            if (!list.Contains(tween))
                list.Add(tween);
        }

        internal void RemoveTween(TweenActionCore tween, UpdateType type, bool unscaled)
        {
            GetList(type, unscaled).Remove(tween);
        }

        internal void ChangeUpdateType(TweenActionCore tween, UpdateType oldType, bool oldUnscaled)
        {
            RemoveTween(tween, oldType, oldUnscaled);
            AddTween(tween);
        }

        internal void Update(UpdateType type, float scaledDt, float unscaledDt)
        {
            UpdateList(GetList(type, false), scaledDt);
            UpdateList(GetList(type, true), unscaledDt);
        }

        private void UpdateList(List<TweenActionCore> list, float dt)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                var tween = list[i];

                if (tween == null || tween.IsDead())
                {
                    if (tween is IPooledTween pooledTween)
						pooledTween.Release();
					
                    list.RemoveAt(i);
                    continue;
                }

                if (!tween.UpdateTween(dt))
                {
                    if (tween is IPooledTween pooledTween)
						pooledTween.Release();
					
                    list.RemoveAt(i);
                }
            }
        }

        private List<TweenActionCore> GetList(UpdateType type, bool unscaled)
        {
            return (type, unscaled) switch
            {
                (UpdateType.Normal, false) => _updateScaled,
                (UpdateType.Normal, true)  => _updateUnscaled,
                (UpdateType.Late, false)   => _lateScaled,
                (UpdateType.Late, true)    => _lateUnscaled,
                (UpdateType.Fixed, false)  => _fixedScaled,
                (UpdateType.Fixed, true)   => _fixedUnscaled,
                _ => _updateScaled
            };
        }

        // --------- Global API ---------

        public static void KillAll(bool complete = false)
        {
            Instance.KillAllInternal(complete);
        }

        public static void Kill(Object target, bool complete = false)
        {
            Instance.KillByTargetInternal(target, complete);
        }

        public static void KillId(object id, bool complete = false)
        {
            Instance.KillByIdInternal(id, complete);
        }

        public static bool IsTweening(Object target)
        {
            return Instance.IsTweeningTargetInternal(target);
        }

        public static bool IsTweeningId(object id)
        {
            return Instance.IsTweeningIdInternal(id);
        }

        private void KillAllInternal(bool complete)
        {
            KillInList(_updateScaled, complete, null, null);
            KillInList(_updateUnscaled, complete, null, null);
            KillInList(_lateScaled, complete, null, null);
            KillInList(_lateUnscaled, complete, null, null);
            KillInList(_fixedScaled, complete, null, null);
            KillInList(_fixedUnscaled, complete, null, null);
        }

        private void KillByTargetInternal(Object target, bool complete)
        {
            if (target == null)
				return;
			
            KillInList(_updateScaled, complete, target, null);
            KillInList(_updateUnscaled, complete, target, null);
            KillInList(_lateScaled, complete, target, null);
            KillInList(_lateUnscaled, complete, target, null);
            KillInList(_fixedScaled, complete, target, null);
            KillInList(_fixedUnscaled, complete, target, null);
        }

        private void KillByIdInternal(object id, bool complete)
        {
            if (id == null) 
				return;
			
            KillInList(_updateScaled, complete, null, id);
            KillInList(_updateUnscaled, complete, null, id);
            KillInList(_lateScaled, complete, null, id);
            KillInList(_lateUnscaled, complete, null, id);
            KillInList(_fixedScaled, complete, null, id);
            KillInList(_fixedUnscaled, complete, null, id);
        }

        private bool IsTweeningTargetInternal(Object target)
        {
            return FindInAnyList(t => t.Target == target);
        }

        private bool IsTweeningIdInternal(object id)
        {
            return FindInAnyList(t => Equals(t.Id, id));
        }

        private bool FindInAnyList(System.Predicate<TweenActionCore> pred)
        {
            return Exists(_updateScaled, pred) ||
                   Exists(_updateUnscaled, pred) ||
                   Exists(_lateScaled, pred) ||
                   Exists(_lateUnscaled, pred) ||
                   Exists(_fixedScaled, pred) ||
                   Exists(_fixedUnscaled, pred);
        }

        private static bool Exists(List<TweenActionCore> list, System.Predicate<TweenActionCore> pred)
		{
			return list.Any(tweenActionCore => tweenActionCore != null && pred(tweenActionCore));
		}

        private static void KillInList(List<TweenActionCore> list, bool complete, Object target, object id)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                var tweenActionCore = list[i];
                if (tweenActionCore == null) { list.RemoveAt(i); continue; }

                bool matchTarget = target == null || tweenActionCore.Target == target;
                bool matchId = id == null || Equals(tweenActionCore.Id, id);

                if (matchTarget && matchId)
                {
                    if (complete) 
						tweenActionCore.Complete(true);
                    else 
						tweenActionCore.Kill();
					
                    list.RemoveAt(i);
                }
            }
        }
    }
}
