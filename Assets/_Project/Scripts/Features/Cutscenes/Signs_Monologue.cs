using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Signs_Monologue : MonoBehaviour
{
    public TextMeshProUGUI TMPtext;
    private bool PlayerInZone = false;
    void Update()
    {
        if (PlayerInZone)
        {
            CutsceneSignals.Instance.StartCutscene("Signs_Monologue");
            Destroy(gameObject);
        }
                
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            PlayerInZone = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            PlayerInZone = false;
    }
}
