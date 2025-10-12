using System;

namespace _Project.Scripts.Features.Transform.Movement
{
    [Serializable]
    public struct Speed
    {
        public float walkSpeed;
        public float runSpeed;

        public Speed(float walkSpeed, float runSpeed)
        {
            this.walkSpeed = walkSpeed;
            this.runSpeed = runSpeed;
        }
    }
}