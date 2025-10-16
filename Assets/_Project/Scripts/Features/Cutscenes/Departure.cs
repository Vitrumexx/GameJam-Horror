using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Departure : MonoBehaviour
{
    private bool PlayerInZone;
    public GameObject player;
    public Transform seat;
    void Update()
    {
        if (PlayerInZone)
            if (Input.GetKeyDown(KeyCode.F))
            {
                CutsceneSignals.Instance.StartCutscene("Departure");
                if (CutsceneSignals.activeCutscene != null)
                {
                    if (CutsceneSignals.activeCutscene.name == "Cutscene_Departure")
                    {
                        player.GetComponent<Rigidbody>().isKinematic = true;
                        player.transform.position = seat.position;
                        player.transform.rotation = seat.rotation;
                        player.transform.SetParent(seat);
                    }
                    Debug.Log(CutsceneSignals.activeCutscene.name);
                }
                player.GetComponentInChildren<AudioSource>().enabled = false;
                gameObject.SetActive(false);
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
