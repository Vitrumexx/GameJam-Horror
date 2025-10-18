using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Features.Inventory;
using _Project.Scripts.Features.Items;
using UnityEngine;

public class CutsceneTest : MonoBehaviour
{
    public Transform seat;
    public GameObject player;
    private void Start()
    {
        CutsceneSignals.Instance.StartCutscene("Intro_Cutscene");
        if (CutsceneSignals.ActiveCutscene != null)
        {
            if (CutsceneSignals.ActiveCutscene.name == "Cutscene_Start")
            {
                player.GetComponent<Rigidbody>().isKinematic = true;
                player.transform.position = seat.position;
                player.transform.rotation = seat.rotation;
                player.transform.SetParent(seat);
            }
            Debug.Log(CutsceneSignals.ActiveCutscene.name);
        }
        player.GetComponentInChildren<AudioSource>().enabled = false;
    }
}
