using TMPro;
using UnityEngine;

public class CashierDialogue : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI TMPtext;
    private bool PlayerInZone = false;
    public TaskCounter counter;

    void Update()
    {
        if (PlayerInZone)
            if (Input.GetKeyDown(KeyCode.F))
            {
                CutsceneSignals.Instance.StartCutscene("CashierDialogue");
                counter.TaskCompleted += 1;
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
        TMPtext.text = "I bought a snack and paid for gas. It was time to continue my ride.";
    }

    public void FreezePlayer()
    {
        player.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void UnfreezePlayer()
    {
        player.GetComponent<Rigidbody>().isKinematic = false;
    }
}
