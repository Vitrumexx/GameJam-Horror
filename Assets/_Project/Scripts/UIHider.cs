using UnityEngine;

public class UIHider : MonoBehaviour
{
    void FixedUpdate()
    {
        if (CutsceneSignals.activeCutscene != null)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }
}
