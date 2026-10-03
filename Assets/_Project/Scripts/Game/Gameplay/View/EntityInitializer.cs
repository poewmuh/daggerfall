using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.View
{
	public class EntityInitializer : MonoBehaviour
	{
		public Entity Entity { get; private set; }
		public World World { get; private set; }
		
		private IEntityBind[] _entityBinds;

		public void Bind(Entity entity, World world)
		{
			Entity = entity;
			World = world;

			_entityBinds = GetComponentsInChildren<IEntityBind>();
			foreach (var bind in _entityBinds)
			{
				bind.Bind(entity, world);
			}
		}

		public void Unbind()
		{
			foreach (var bind in _entityBinds)
			{
				bind.Unbind(Entity, World);
			}

			Entity = default;
			World = null;
		}
	}
}