using TriInspector;
using UnityEngine;

namespace Game.Gameplay.Combat
{
	[CreateAssetMenu(fileName = "WeaponConfig", menuName = "_Project/Gameplay/Weapon Config")]
	[DeclareBoxGroup("Identity", Title = "Identity")]
	[DeclareBoxGroup("Attack", Title = "Attack")]
	[DeclareBoxGroup("Charge", Title = "Charge")]
	[DeclareBoxGroup("Timing", Title = "Timing")]
	[DeclareBoxGroup("Presentation", Title = "Overlay Presentation")]
	[DeclareBoxGroup("Delivery", Title = "Attack Delivery")]
	public sealed class WeaponConfig : ScriptableObject
	{
		[Group("Identity")]
		[SerializeField] private string _id = "weapon";
		[Group("Identity")]
		[SerializeField] private string _displayName = "Weapon";
		[Group("Identity")]
		[SerializeField] private WeaponType _weaponType;

		[Group("Attack")]
		[SerializeField] private AttackInputMode _inputMode;
		[Group("Attack")]
		[SerializeField, Min(0f)] private float _baseDamage = 10f;

		[Group("Charge"), ShowIf(nameof(_inputMode), AttackInputMode.Charge)]
		[SerializeField, Min(0.01f)] private float _maxChargeDuration = 1.5f;
		[Group("Charge"), ShowIf(nameof(_inputMode), AttackInputMode.Charge)]
		[SerializeField, Min(0f)] private float _maximumChargedDamage = 30f;

		[Group("Timing")]
		[SerializeField, Min(0.01f)] private float _swingDuration = 0.3f;
		[Group("Timing")]
		[SerializeField, Range(0f, 1f)] private float _hitTime = 0.45f;
		[Group("Timing")]
		[SerializeField, Min(0f)] private float _recoveryDuration = 0.35f;

		[Group("Presentation")]
		[SerializeField, PreviewObject(Height = 120)] private Sprite _weaponSprite;
		[Group("Presentation")]
		[SerializeField] private Vector2 _chargeOffset = new(0f, -45f);
		[Group("Presentation")]
		[SerializeField] private Vector2 _swingOffset = new(-260f, 40f);
		[Group("Presentation")]
		[SerializeField] private float _chargeRotation = -8f;
		[Group("Presentation")]
		[SerializeField] private float _swingRotation = 75f;

		[Group("Delivery")]
		[ValidateInput(nameof(ValidateDelivery))]
		[SerializeReference] private IAttackDelivery _delivery = new MeleeAttackDelivery();

		public string Id => _id;
		public string DisplayName => _displayName;
		public WeaponType WeaponType => _weaponType;
		public AttackInputMode InputMode => _inputMode;
		public float MaxChargeDuration => _maxChargeDuration;
		public float SwingDuration => _swingDuration;
		public float HitTime => _hitTime;
		public float RecoveryDuration => _recoveryDuration;
		public Sprite WeaponSprite => _weaponSprite;
		public Vector2 ChargeOffset => _chargeOffset;
		public Vector2 SwingOffset => _swingOffset;
		public float ChargeRotation => _chargeRotation;
		public float SwingRotation => _swingRotation;
		public IAttackDelivery Delivery => _delivery;

		public float EvaluateDamage(float normalizedCharge)
		{
			if (_inputMode == AttackInputMode.Instant)
				return _baseDamage;

			return Mathf.Lerp(_baseDamage, _maximumChargedDamage, Mathf.Clamp01(normalizedCharge));
		}

		private void OnValidate()
		{
			_maximumChargedDamage = Mathf.Max(_baseDamage, _maximumChargedDamage);
		}

		private TriValidationResult ValidateDelivery()
		{
			return _delivery == null
				? TriValidationResult.Error("An attack delivery must be selected.")
				: TriValidationResult.Valid;
		}
	}
}
