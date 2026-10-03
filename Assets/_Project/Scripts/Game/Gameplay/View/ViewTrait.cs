using System;
using Game.Gameplay.Agent;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.View
{
	[Serializable]
	public sealed class ViewTrait : IAgentTrait
	{
		[SerializeField] private string _assetId;

		public void Install(Entity entity, World world)
		{
			world.GetStash<AssetPath>().Set(entity, new AssetPath { Value = _assetId });
		}
	}
}
