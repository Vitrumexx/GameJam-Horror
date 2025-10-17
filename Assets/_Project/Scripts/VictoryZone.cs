using TMPro;
using UnityEngine;

public class VictoryZone : MonoBehaviour
{
    public TextMeshProUGUI TMPtext;
    private bool PlayerInZone = false;
    public GameObject planks;
    public GameObject player;
    public Camera cam;


    private void Update()
    {
        if (planks.active)
            
            gameObject.GetComponent<Collider>().enabled = true;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cam.gameObject.SetActive(true);
            PlayerInZone = true;
            CutsceneSignals.Instance.StartCutscene("Victory");
        } 
    }
}
