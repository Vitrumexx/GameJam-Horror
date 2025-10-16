using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trashbox_Screamer : MonoBehaviour
{
    private bool PlayerInZone = false;
    void OnTriggetStay(Collider other)
    {
        if (other.CompareTag("Player"))
            PlayerInZone = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Запускаем катсцену
        }
    }
}
