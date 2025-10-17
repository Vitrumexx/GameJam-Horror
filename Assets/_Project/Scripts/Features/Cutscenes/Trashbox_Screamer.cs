using _Project.Scripts.Features.Tasks;
using TMPro;

public class Trashbox_Screamer : MonoTask
{
    public TextMeshProUGUI TMPtext;
    public TaskCounter counter;
    
    public void StartScreaming()
    {
        PlayerNotifier.UpdateTask(taskId, true);
        
        CutsceneSignals.Instance.StartCutscene("Trashbox_Screamer");
        counter.TaskCompleted += 1;
    }

    public void Phrase()
    {
        TMPtext.text = "This bum came out so unexpectable, I almost shit my pants.";
    }
}
