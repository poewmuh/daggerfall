namespace Game.Gameplay.Combat
{
	public readonly struct WeaponPresentationMessage
	{
		public WeaponPresentation Presentation { get; }
		public WeaponActionPhase Phase { get; }
		public float PhaseProgress { get; }
		public float Charge { get; }

		public WeaponPresentationMessage(WeaponPresentation presentation, WeaponActionPhase phase, float phaseProgress, float charge)
		{
			Presentation = presentation;
			Phase = phase;
			PhaseProgress = phaseProgress;
			Charge = charge;
		}
	}
}
