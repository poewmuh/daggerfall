using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.View
{
	public struct View : IComponent
	{
		public EntityInitializer Value;
	}

	public struct AssetPath : IComponent
	{
		public string Value;
	}

	public struct Billboard : IComponent
	{
		public Transform Value;
	}
}
