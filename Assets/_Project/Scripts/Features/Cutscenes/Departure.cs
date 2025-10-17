using UnityEngine;

public class Departure : MonoBehaviour
{
    public GameObject player;
    public Transform seat;

    public void OnDeparture()
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
    }
}
