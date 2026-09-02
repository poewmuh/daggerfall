using UnityEngine.InputSystem;

namespace Game.Application.InputSystem.InputActions
{
    public class PlayerInputActions : InputActionsBase, InputSystemActions.IPlayerActions
    {
        public PlayerInputActions(IInputSystem inputSystem) : base(inputSystem)
        {
        }

        void InputSystemActions.IPlayerActions.OnMove(InputAction.CallbackContext context)
        {
	        TriggerAction(context, InputSystem.InputEvents.OnMove);
        }

        void InputSystemActions.IPlayerActions.OnLook(InputAction.CallbackContext context)
        {
	        TriggerAction(context, InputSystem.InputEvents.OnLook);
        }

        void InputSystemActions.IPlayerActions.OnAttack(InputAction.CallbackContext context)
        {
	        TriggerAction(context, InputSystem.InputEvents.OnAttack);
        }

        void InputSystemActions.IPlayerActions.OnInteract(InputAction.CallbackContext context)
        {
	        TriggerAction(context, InputSystem.InputEvents.OnInteract);
        }

        void InputSystemActions.IPlayerActions.OnDash(InputAction.CallbackContext context)
        {
	        TriggerAction(context, InputSystem.InputEvents.OnDash);
        }

        void InputSystemActions.IPlayerActions.OnPrevious(InputAction.CallbackContext context) { }

        void InputSystemActions.IPlayerActions.OnNext(InputAction.CallbackContext context) { }
    }
}