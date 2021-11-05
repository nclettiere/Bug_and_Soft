using UnityEngine.InputSystem;

namespace Managers
{
    public interface IJoystickInput
    {
        public void OnJoystickMovement(InputAction.CallbackContext context);
    }
}