using TriInspector;
using UnityEngine;

namespace Game.Gameplay.Movement
{
	[CreateAssetMenu(fileName = "DashConfig", menuName = "_Project/Gameplay/Dash Config")]
	[DeclareBoxGroup("Dash", Title = "Dash")]
	public sealed class DashConfig : ScriptableObject
	{
		[Group("Dash")]
		[SerializeField, Min(0.01f)] private float _distance = 4f;
		[Group("Dash")]
		[SerializeField, Min(0.01f)] private float _duration = 0.18f;
		[Group("Dash")]
		[SerializeField, Min(0f)] private float _cooldown = 1f;

		public float Distance => _distance;
		public float Duration => _duration;
		public float Cooldown => _cooldown;
	}
}
