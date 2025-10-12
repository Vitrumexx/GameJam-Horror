using UnityEngine;

namespace _Project.Scripts.Features.Transform.Movement.KeyboardMovement
{
    public class KeyboardMovementConfig : ScriptableObject
    {
        public Speed speed = new Speed(8, 15);
        public KeyboardMovement keyboardMovement = new KeyboardMovement(
            KeyCode.LeftShift, "Horizontal", "Vertical");
    }
}