using TriInspector;
using UnityEngine;

namespace Game.Gameplay.Combat
{
	[CreateAssetMenu(fileName = "WeaponPresentation", menuName = "_Project/Gameplay/Weapon Presentation")]
	public sealed class WeaponPresentation : ScriptableObject
	{
		[SerializeField, PreviewObject(Height = 120)] private Sprite _sprite;
		[SerializeField] private Vector2 _chargeOffset = new(0f, -45f);
		[SerializeField] private Vector2 _swingOffset = new(-260f, 40f);
		[SerializeField] private float _chargeRotation = -8f;
		[SerializeField] private float _swingRotation = 75f;

		public Sprite Sprite => _sprite;
		public Vector2 ChargeOffset => _chargeOffset;
		public Vector2 SwingOffset => _swingOffset;
		public float ChargeRotation => _chargeRotation;
		public float SwingRotation => _swingRotation;
	}
}
