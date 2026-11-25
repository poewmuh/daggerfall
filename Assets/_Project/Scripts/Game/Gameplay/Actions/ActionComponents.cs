using System;
using Scellecs.Morpeh;

namespace Game.Gameplay.Actions
{
	public struct ActionBlockers : IComponent
	{
		public ActionBlockReason Value;
	}

	[Flags]
	public enum ActionBlockReason : byte
	{
		None = 0,
		Dead = 1 << 0,
	}
}
