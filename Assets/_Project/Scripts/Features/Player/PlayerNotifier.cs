using System;
using UnityEngine;

namespace _Project.Scripts.Features.Player
{
    public class PlayerNotifier : MonoBehaviour
    {
        public void NotifyPlayer(string msg)
        {
            Debug.Log(msg);
        }

        public void NotifyPlayer(string msg, Sprite sprite)
        {
            Debug.Log($"{msg} + sprite: {sprite.name}");
        }
    }
}