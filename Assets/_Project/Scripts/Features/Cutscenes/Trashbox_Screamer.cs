using UnityEngine;

public class Trashbox_Screamer : MonoBehaviour
{
    private bool PlayerInZone = false;

    void Update()
    {
        if (PlayerInZone)
            if (Input.GetKeyDown(KeyCode.E))
            {
                CutsceneSignals.Instance.StartCutscene("Trashbox_Screamer");
                Destroy(gameObject);
            }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
            PlayerInZone = true;
    }

   
}
