using _Project.Scripts.Features.Tasks;
using TMPro;
using UnityEngine;

public class CashierDialogue : MonoTask
{
    public GameObject player;
    public TextMeshProUGUI TMPtext;
    public TaskCounter counter;

    public void OnDialogue()
    {
        PlayerNotifier.UpdateTask(taskId, true);
        
        CutsceneSignals.Instance.StartCutscene("CashierDialogue");
        counter.TaskCompleted += 1;
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
