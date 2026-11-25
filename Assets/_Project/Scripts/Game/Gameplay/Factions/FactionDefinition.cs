using System;
using UnityEngine;

namespace Game.Gameplay.Factions
{
	[CreateAssetMenu(fileName = "Faction", menuName = "_Project/Gameplay/Faction")]
	public sealed class FactionDefinition : ScriptableObject
	{
		[SerializeField] private FactionDefinition[] _hostileTo = Array.Empty<FactionDefinition>();

		public bool IsHostileTo(FactionDefinition other)
		{
			return other != null && Array.IndexOf(_hostileTo, other) >= 0;
		}
	}
}
