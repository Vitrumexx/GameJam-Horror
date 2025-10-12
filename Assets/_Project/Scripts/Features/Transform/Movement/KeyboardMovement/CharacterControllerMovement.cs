using UnityEngine;

namespace _Project.Scripts.Features.Transform.Movement.KeyboardMovement
{
    public class CharacterControllerMovement : MonoBehaviour
    {
        public KeyboardMovementConfig config;
        public CharacterController characterController;

        public void Update()
        {
            Move();
        }

        private void Move()
        {
            var moveX = Input.GetAxis(config.keyboardMovement.horizontalAxis);
            var moveZ = Input.GetAxis(config.keyboardMovement.verticalAxis);

            var move = transform.right * moveX + transform.forward * moveZ;
            var speed = Input.GetKey(config.keyboardMovement.runKey) 
                ? config.speed.runSpeed 
                : config.speed.walkSpeed;

            characterController.Move(move * (speed * Time.deltaTime));
        }
    }
}