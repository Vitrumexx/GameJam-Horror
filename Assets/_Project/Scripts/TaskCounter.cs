using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskCounter : MonoBehaviour
{
    public int TotalTasks = 2;
    public int TaskCompleted;
    public GameObject DepartureScriptableZone;


    private void Start()
    {
        TaskCompleted = 0;
        DepartureScriptableZone.SetActive(false);   
    }
    private void Update()
    {
        if (TaskCompleted >= TotalTasks)
        {
            DepartureScriptableZone.SetActive(true);
        }
    }
}
