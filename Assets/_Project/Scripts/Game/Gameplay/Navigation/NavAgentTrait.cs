using System;
using Game.Gameplay.Agent;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Navigation
{
	[Serializable]
	public sealed class NavAgentTrait : IAgentTrait
	{
		private const int MaxCorners = 32;

		[SerializeField, Min(0.05f)] private float _repathInterval = 0.25f;
		[SerializeField, Min(0.01f)] private float _cornerReachDistance = 0.25f;
		[SerializeField, Min(0.1f)] private float _sampleDistance = 2f;

		public void Install(Entity entity, World world)
		{
			world.GetStash<NavAgent>().Set(entity, new NavAgent
			{
				RepathInterval = _repathInterval,
				CornerReachDistance = _cornerReachDistance,
				SampleDistance = _sampleDistance,
			});

			world.GetStash<NavPath>().Set(entity, new NavPath { Corners = new Vector3[MaxCorners] });
		}
	}
}
