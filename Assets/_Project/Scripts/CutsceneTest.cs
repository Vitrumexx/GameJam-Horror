using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTest : MonoBehaviour
{
    public Transform seat;
    public GameObject player;
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.C))
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
}
