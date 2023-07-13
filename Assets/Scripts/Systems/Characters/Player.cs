using UnityEngine;
using UnityEngine.InputSystem;

namespace Systems.Characters
{
    public class Player : Character
    {
        public void Move(InputAction.CallbackContext context)
        {
            var delta = context.ReadValue<Vector2>();
            
            Movement.PerformMove(delta);
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;
            
            Movement.PerformJump();
        }
    }
}