using Game.Gameplay.View;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.Movement.Binds
{
	public class RigidbodyBind : MonoBehaviour, IEntityBind
	{
		[SerializeField] private Rigidbody _rigidbody;
		
		void IEntityBind.Bind(Entity entity, World world)
		{
			world.GetStash<RigidbodyComponent>().Set(entity, new RigidbodyComponent { Value = _rigidbody });
		}

		void IEntityBind.Unbind(Entity entity, World world)
		{
			world.GetStash<RigidbodyComponent>().Remove(entity);
		}
	}
}