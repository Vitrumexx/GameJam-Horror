using _Project.Scripts.Features.Player;
using UnityEngine;

namespace _Project.Scripts.Features.Tasks
{
    public class MonoTask : MonoBehaviour
    {
        public string taskId;
        [TextArea] public string task;

        protected PlayerNotifier PlayerNotifier;
        
        protected virtual void Start()
        {
            PlayerNotifier = FindAnyObjectByType<PlayerNotifier>();
            PlayerNotifier?.UpdateTask(taskId, false, task);
        }
    }
}