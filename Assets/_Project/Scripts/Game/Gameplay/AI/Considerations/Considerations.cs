using System;
using UnityEngine;

namespace Game.Gameplay.AI
{
	[Serializable]
	public sealed class ConstantConsideration : IConsideration
	{
		[SerializeField, Range(0f, 1f)] private float _value = 0.2f;

		public ConstantConsideration() { }

		public ConstantConsideration(float value)
		{
			_value = value;
		}

		public float Evaluate(in AiContext context) => _value;
	}

	[Serializable]
	public sealed class HasTargetConsideration : IConsideration
	{
		public float Evaluate(in AiContext context) => context.HasTarget ? 1f : 0f;
	}

	[Serializable]
	public sealed class TargetVisibleConsideration : IConsideration
	{
		public float Evaluate(in AiContext context) => context.TargetVisible ? 1f : 0f;
	}

	[Serializable]
	public sealed class DistanceToTargetConsideration : IConsideration
	{
		[SerializeField] private AnimationCurve _curve = AnimationCurve.Linear(0f, 1f, 1f, 1f);

		public float Evaluate(in AiContext context)
		{
			if (!context.HasTarget || context.PerceptionRange <= 0f)
				return 0f;

			return _curve.Evaluate(Mathf.Clamp01(context.DistanceToTarget / context.PerceptionRange));
		}
	}

	[Serializable]
	public sealed class TimeSinceSeenConsideration : IConsideration
	{
		[SerializeField] private AnimationCurve _curve = AnimationCurve.Linear(0f, 1f, 1f, 0f);

		public float Evaluate(in AiContext context)
		{
			if (!context.HasTarget || context.MemoryDuration <= 0f)
				return 0f;

			return _curve.Evaluate(Mathf.Clamp01(context.TimeSinceSeen / context.MemoryDuration));
		}
	}

	[Serializable]
	public sealed class HealthConsideration : IConsideration
	{
		[SerializeField] private AnimationCurve _curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		public float Evaluate(in AiContext context) => _curve.Evaluate(Mathf.Clamp01(context.HealthRatio));
	}
}
