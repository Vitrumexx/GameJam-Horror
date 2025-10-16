using TMPro;
using UnityEngine;

public class Trashbox_Screamer : MonoBehaviour
{
    public TextMeshProUGUI TMPtext;
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

    public void Phrase()
    {
        TMPtext.text = "This bum came out so unexpectable, I almost shit my pants.";
    }
}
