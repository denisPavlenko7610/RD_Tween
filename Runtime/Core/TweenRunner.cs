using UnityEngine;

namespace RD_Tween.Runtime
{
	internal sealed class TweenRunner : MonoBehaviour
	{
		private TweenManager _manager;

		internal void Init(TweenManager manager) => _manager = manager;

		private void Update()
		{
			_manager?.Update(UpdateType.Normal, Time.deltaTime, Time.unscaledDeltaTime);
		}

		private void LateUpdate()
		{
			_manager?.Update(UpdateType.Late, Time.deltaTime, Time.unscaledDeltaTime);
		}

		private void FixedUpdate()
		{
			_manager?.Update(UpdateType.Fixed, Time.fixedDeltaTime, Time.fixedUnscaledDeltaTime);
		}
	}
}
