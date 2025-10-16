using UnityEngine;

public class Trashbox_Screamer : MonoBehaviour
{
    private bool PlayerInZone = false;

    void Update()
    {
        if (PlayerInZone)
            if (Input.GetKeyDown(KeyCode.F))
            {
                CutsceneSignals.Instance.StartCutscene("Trashbox_Screamer");
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
