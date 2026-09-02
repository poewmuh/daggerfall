using UnityEngine;

namespace Game.Gameplay.Core
{
	public abstract class ObjectInitializer : MonoBehaviour
	{
		[SerializeField] private bool _initializeOnStart = true;
		
		private void Start()
		{
			if (_initializeOnStart) 
				Initialize();
		}

		protected abstract void InitializeInternal();

		private void Initialize()
		{
			InitializeInternal();
			InitComponents();
		}
		
		private void InitComponents()
		{
			var components = GetComponents<IBaseComponent>();
			foreach (var component in components)
			{
				component.Init();
			}
		}
	}
}