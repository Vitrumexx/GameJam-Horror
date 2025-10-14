using System;
using UnityEngine;

namespace _Project.Scripts.Features.Enemy
{
    public class EnemyNotifier : MonoBehaviour
    {
        public void NotifyEnemyAboutDroppedItem(Vector3 pickUpPosition)
        {
            Debug.Log($"Enemy notified at {pickUpPosition}");
        }
    }
}