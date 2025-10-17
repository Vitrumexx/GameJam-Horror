using _Project.Scripts.Features.Interactable;
using _Project.Scripts.Features.Player;
using UnityEngine;

public class TaskCounter : MonoBehaviour
{
    public int TotalTasks = 2;
    public Departure departure;

    private int _taskCompleted = 0;
    
    public int TaskCompleted
    {
        get => _taskCompleted;
        set
        {
            _taskCompleted = value;
            
            if (TaskCompleted >= TotalTasks)
            {
                departure.GetComponent<FuncInvokerInteractable>().isInteractable = true;
                
                var playerNotification = FindAnyObjectByType<PlayerNotifier>();    
                playerNotification?.ClearTasks();
                playerNotification?.AddTask("abobababa", "Go back to the car", false);
            }
        }
    }
}
