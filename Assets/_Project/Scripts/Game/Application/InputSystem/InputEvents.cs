using Game.Application.InputSystem.InputActions;
using R3;

namespace Game.Application.InputSystem
{
	public class InputEvents
	{
		public Subject<InputActionData> OnPause { get; } = new();
		public Subject<InputActionData> OnSkip { get; } = new();
		
		public Subject<InputVectorActionData> OnLook { get; } = new();
		public Subject<InputVectorActionData> OnMove { get; } = new();
		public Subject<InputActionData> OnDash { get; } = new();
		
		public Subject<InputActionData> OnAttack { get; } = new();
		
		public Subject<InputActionData> OnInteract { get; } = new();
	}
}