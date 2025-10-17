using UnityEngine;

public class UIHider : MonoBehaviour
{
    void Update()
    {
        gameObject.SetActive(!CutsceneSignals.activeCutscene);
    }
}
