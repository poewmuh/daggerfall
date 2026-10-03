using Scellecs.Morpeh;

namespace Game.Gameplay.View
{
	public interface IEntityBind
	{
		void Bind(Entity entity, World world);
		void Unbind(Entity entity, World world);
	}
}