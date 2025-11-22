namespace RD_Tween.Runtime
{
	public enum UpdateType { Normal, Late, Fixed }
	public enum LoopType { Restart, Yoyo, Incremental }

	public interface ITween
	{
		bool UpdateTween(float deltaTime);
		bool IsDead();
	}

	public interface IControllableTween : ITween
	{
		float Duration { get; }
		void Goto(float time, bool play = false);
		void GotoNormalized(float t01, bool play = false);
		void Rewind(bool play = false);
		void Complete(bool withCallbacks = true);
	}

	internal interface IPooledTween
	{
		void Release();
	}
}
