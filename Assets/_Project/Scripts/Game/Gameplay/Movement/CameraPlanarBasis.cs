using UnityEngine;

namespace Game.Gameplay.Movement
{
	public readonly struct CameraPlanarBasis
	{
		public readonly Vector3 Right;
		public readonly Vector3 Forward;

		private CameraPlanarBasis(Vector3 right, Vector3 forward)
		{
			Right = right;
			Forward = forward;
		}

		public static CameraPlanarBasis From(Camera camera)
		{
			var right = Vector3
				.ProjectOnPlane(camera.transform.right, Vector3.up)
				.normalized;
			var forward = Vector3
				.Cross(right, Vector3.up)
				.normalized;

			return new CameraPlanarBasis(right, forward);
		}
	}
}
