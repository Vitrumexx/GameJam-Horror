using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomeHome : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CutsceneSignals.Instance.StartCutscene("WelcomeHome_Cutscene");
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
