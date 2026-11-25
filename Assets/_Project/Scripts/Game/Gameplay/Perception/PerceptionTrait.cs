using System;
using Game.Gameplay.Agent;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Perception
{
	[Serializable]
	public sealed class PerceptionTrait : IAgentTrait
	{
		[SerializeField, Min(0f)] private float _range = 12f;
		[SerializeField, Min(0f)] private float _proximityRadius = 1.5f;
		[SerializeField, Range(1f, 360f)] private float _fieldOfView = 120f;
		[SerializeField, Min(0f)] private float _eyeHeight = 0.6f;
		[SerializeField, Min(0f)] private float _targetHeight = 1f;
		[SerializeField, Min(0f)] private float _memoryDuration = 5f;
		[SerializeField, Min(0.02f)] private float _interval = 0.2f;
		[SerializeField] private LayerMask _obstacleMask = ~((1 << 2) | (1 << 6));

		public void Install(Entity entity, World world)
		{
			world.GetStash<PerceptionParameters>().Set(entity, new PerceptionParameters
			{
				Range = _range,
				ProximityRadius = _proximityRadius,
				HalfFovCos = Mathf.Cos(_fieldOfView * 0.5f * Mathf.Deg2Rad),
				EyeHeight = _eyeHeight,
				TargetHeight = _targetHeight,
				MemoryDuration = _memoryDuration,
				Interval = _interval,
				Timer = UnityEngine.Random.Range(0f, _interval),
				ObstacleMask = _obstacleMask,
			});

			world.GetStash<PerceivedTarget>().Set(entity);
		}
	}
}
