using System;
using UnityEngine;

namespace _Project.Scripts.Features.Transform.Movement.KeyboardMovement
{
    [Serializable]
    public struct KeyboardMovement
    {
        public KeyCode runKey;
        public string horizontalAxis;
        public string verticalAxis;

        public KeyboardMovement(KeyCode runKey, string horizontalAxis, string verticalAxis)
        {
            this.runKey = runKey;
            this.horizontalAxis = horizontalAxis;
            this.verticalAxis = verticalAxis;
        }
    }
}