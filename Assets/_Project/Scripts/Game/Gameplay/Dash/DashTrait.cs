using System;
using Game.Gameplay.Agent;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Dash
{
	[Serializable]
	public sealed class DashTrait : IAgentTrait
	{
		[SerializeField, Min(0f)] private float _distance = 4f;
		[SerializeField, Min(0.02f)] private float _duration = 0.2f;
		[SerializeField, Min(0f)] private float _cooldown = 1f;
		[SerializeField] private DashFallbackDirection _fallbackDirection = DashFallbackDirection.Facing;

		public void Install(Entity entity, World world)
		{
			world.GetStash<DashParameters>().Set(entity, new DashParameters
			{
				Distance = _distance,
				Duration = _duration,
				Cooldown = _cooldown,
				FallbackDirection = _fallbackDirection,
			});
		}
	}
}
