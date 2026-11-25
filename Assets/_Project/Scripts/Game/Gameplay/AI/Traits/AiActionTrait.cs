using System;
using Game.Gameplay.Agent;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.AI.Traits
{
	[Serializable]
	public abstract class AiActionTrait : IAgentTrait
	{
		[SerializeField, Min(0f)] private float _weight = 1f;
		[SerializeReference] private IConsideration[] _considerations;

		protected AiActionTrait()
		{
			_weight = DefaultWeight;
			_considerations = CreateDefaultConsiderations();
		}

		protected virtual float DefaultWeight => 1f;

		public void Install(Entity owner, World world)
		{
			var action = world.CreateEntity();

			world.GetStash<AiAction>().Set(action, new AiAction
			{
				Owner = owner,
				Weight = _weight,
				Considerations = _considerations ?? Array.Empty<IConsideration>(),
			});

			InstallAction(action, owner, world);
		}

		protected abstract IConsideration[] CreateDefaultConsiderations();

		protected abstract void InstallAction(Entity action, Entity owner, World world);
	}
}
