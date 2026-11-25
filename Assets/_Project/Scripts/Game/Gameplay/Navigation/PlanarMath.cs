using UnityEngine;

namespace Game.Gameplay.Navigation
{
	public static class PlanarMath
	{
		public static Vector3 Flatten(Vector3 vector)
		{
			return new Vector3(vector.x, 0f, vector.z);
		}

		public static float Distance(Vector3 a, Vector3 b)
		{
			return Flatten(b - a).magnitude;
		}
	}
}
