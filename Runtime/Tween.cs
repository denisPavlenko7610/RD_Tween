using System.Collections.Generic;

namespace RD_Tween.Runtime
{
    public class Tween
    {
        private static Tween _instance;
        private List<TweenAction> _activeTweens = new();

        public static Tween Instance => _instance ??= new Tween();

        public void Update(float deltaTime)
        {
            for (int i = _activeTweens.Count - 1; i >= 0; i--)
            {
                if (_activeTweens[i] == null || _activeTweens[i].IsDead())
                {
                    _activeTweens.RemoveAt(i);
                    return;
                }
                
                if (_activeTweens[i].UpdateTween(deltaTime))
                    continue;
            
                _activeTweens.RemoveAt(i);
            }
        }

        public void AddTween(TweenAction action) => _activeTweens.Add(action);
    }
}