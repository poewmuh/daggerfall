using System;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game.Gameplay.View.Systems
{
	public sealed class BillboardSystem : ILateSystem
	{
		private const float MinSqrMagnitude = 0.0001f;

		private readonly Camera _camera;

		private Filter _filter;
		private Stash<Billboard> _billboardStash;

		public World World { get; set; }

		public BillboardSystem(Camera camera)
		{
			_camera = camera;
		}

		void IInitializer.OnAwake()
		{
			_filter = World.Filter
				.With<Billboard>()
				.Build();

			_billboardStash = World.GetStash<Billboard>();
		}

		void ISystem.OnUpdate(float deltaTime)
		{
			var cameraPosition = _camera.transform.position;

			foreach (var entity in _filter)
			{
				var target = _billboardStash.Get(entity).Value;

				if (!target)
					continue;

				var direction = target.position - cameraPosition;
				direction.y = 0f;

				if (direction.sqrMagnitude > MinSqrMagnitude)
					target.rotation = Quaternion.LookRotation(direction, Vector3.up);
			}
		}

		void IDisposable.Dispose() { }
	}
}
