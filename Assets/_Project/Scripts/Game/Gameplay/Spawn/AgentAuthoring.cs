using Game.Gameplay.Agent;
using Game.Gameplay.View;
using TriInspector;
using UnityEngine;

namespace Game.Gameplay.Spawn
{
	[RequireComponent(typeof(EntityInitializer))]
	public sealed class AgentAuthoring : MonoBehaviour
	{
		[Required]
		[SerializeField] private AgentConfig _config;

		public AgentConfig Config => _config;
		public EntityInitializer EntityInitializer => GetComponent<EntityInitializer>();
	}
}
