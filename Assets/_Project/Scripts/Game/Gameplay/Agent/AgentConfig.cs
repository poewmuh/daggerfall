using Game.Gameplay.Entities;
using UnityEngine;

namespace Game.Gameplay.Agent
{
	[CreateAssetMenu(fileName = "AgentConfig", menuName = "_Project/Gameplay/Agent Config")]
	public sealed class AgentConfig : EntityConfig<IAgentTrait> { }
}
