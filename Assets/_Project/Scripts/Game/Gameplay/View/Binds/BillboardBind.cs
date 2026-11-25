using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.View.Binds
{
	public sealed class BillboardBind : MonoBehaviour, IEntityBind
	{
		[SerializeField] private Transform _target;

		void IEntityBind.Bind(Entity entity, World world)
		{
			world.GetStash<Billboard>().Set(entity, new Billboard { Value = _target });
		}

		void IEntityBind.Unbind(Entity entity, World world)
		{
			world.GetStash<Billboard>().Remove(entity);
		}
	}
}
