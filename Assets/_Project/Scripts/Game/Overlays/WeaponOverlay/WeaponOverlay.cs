using Game.Gameplay.Combat;
using MessagePipe;
using R3;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Game.Overlays.WeaponOverlay
{
	public class WeaponOverlay : BaseOverlay
	{
		[SerializeField] private RectTransform _weaponTransform;
		[SerializeField] private Image _weaponImage;

		[Inject] private ISubscriber<WeaponPresentationMessage> _presentationSubscriber;

		private Vector2 _idlePosition;
		private WeaponPresentation _presentation;

		private void Awake()
		{
			_idlePosition = _weaponTransform.anchoredPosition;
			_presentationSubscriber.Subscribe(ApplyState).AddTo(this);
		}

		private void ApplyState(WeaponPresentationMessage message)
		{
			if (_presentation != message.Presentation)
			{
				_presentation = message.Presentation;
				_weaponImage.sprite = _presentation.Sprite;
			}

			switch (message.Phase)
			{
				case WeaponActionPhase.Idle:
					SetPose(_idlePosition, 0f);
					break;
				case WeaponActionPhase.Charging:
					SetPose(
						Vector2.Lerp(_idlePosition, _idlePosition + _presentation.ChargeOffset, message.PhaseProgress),
						Mathf.Lerp(0f, _presentation.ChargeRotation, message.PhaseProgress));
					break;
				case WeaponActionPhase.Swing:
					ApplySwing(message.PhaseProgress);
					break;
				case WeaponActionPhase.Recovery:
					SetPose(
						Vector2.Lerp(_idlePosition + _presentation.SwingOffset, _idlePosition, message.PhaseProgress),
						Mathf.Lerp(_presentation.SwingRotation, 0f, message.PhaseProgress));
					break;
			}
		}

		private void ApplySwing(float progress)
		{
			var easedProgress = Mathf.SmoothStep(0f, 1f, progress);
			var position = Vector2.Lerp(_idlePosition + _presentation.ChargeOffset, _idlePosition + _presentation.SwingOffset, easedProgress);
			var rotation = Mathf.Lerp(_presentation.ChargeRotation, _presentation.SwingRotation, easedProgress);
			SetPose(position, rotation);
		}

		private void SetPose(Vector2 position, float rotation)
		{
			_weaponTransform.anchoredPosition = position;
			_weaponTransform.localRotation = Quaternion.Euler(0f, 0f, rotation);
		}
	}
}
