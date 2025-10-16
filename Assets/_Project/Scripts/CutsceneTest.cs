using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTest : MonoBehaviour
{
    public Transform seat;
    public GameObject player;
    private void Start()
    {
        CutsceneSignals.Instance.StartCutscene("Intro_Cutscene");
        if (CutsceneSignals.activeCutscene != null)
        {
            if (CutsceneSignals.activeCutscene.name == "Cutscene_Start")
            {
                player.GetComponent<Rigidbody>().isKinematic = true;
                player.transform.position = seat.position;
                player.transform.rotation = seat.rotation;
                player.transform.SetParent(seat);
            }
            Debug.Log(CutsceneSignals.activeCutscene.name);
        }
        player.GetComponentInChildren<AudioSource>().enabled = false;
    }
}
