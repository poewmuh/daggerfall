namespace Game.Gameplay.Combat
{
	public readonly struct WeaponPresentationMessage
	{
		public WeaponConfig Config { get; }
		public WeaponActionPhase Phase { get; }
		public float PhaseProgress { get; }
		public float Charge { get; }

		public WeaponPresentationMessage(WeaponConfig config, WeaponActionPhase phase, float phaseProgress, float charge)
		{
			Config = config;
			Phase = phase;
			PhaseProgress = phaseProgress;
			Charge = charge;
		}
	}
}
