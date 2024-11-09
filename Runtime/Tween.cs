using System.Collections.Generic;

namespace RD_Tween.Runtime
{
    public class TweenManager
    {
        private static TweenManager _instance;
        private List<TweenAction> _activeTweens = new();

        public static TweenManager Instance => _instance ??= new TweenManager();

        public void Update(float deltaTime)
        {
            if (_activeTweens.Count == 0)
                return;
            
            for (int i = _activeTweens.Count - 1; i >= 0; i--)
            {
                if (_activeTweens[i].UpdateTween(deltaTime))
                    continue;
            
                _activeTweens.RemoveAt(i);
            }
        }

        public void AddTween(TweenAction action) => _activeTweens.Add(action);
    }
}