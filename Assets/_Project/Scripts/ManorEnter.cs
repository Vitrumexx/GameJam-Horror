using UnityEngine;

public class ManorEnter : MonoBehaviour
{
    public GameObject planks;
    public GameObject hunter;

    private void Start()
    {
        planks.SetActive(false);
        hunter.SetActive(false);
    }
    void OnTriggerEnter(Collider other)
    {
        planks.SetActive(true);
        hunter.SetActive(true);
    }
}
