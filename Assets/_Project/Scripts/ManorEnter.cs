using UnityEngine;

public class ManorEnter : MonoBehaviour
{
    public GameObject planks;
    public GameObject hunter;
    public GameObject door1;
    public GameObject door2;

    public AudioClip music;
    public AudioSource playerSource;

    private GameObject _newDoor1;
    private GameObject _newDoor2;
    
    private void Start()
    {
        planks.SetActive(false);
        hunter.SetActive(false);
        
        _newDoor1 = Instantiate(door1);
        _newDoor2 = Instantiate(door2);
        
        door1.SetActive(false);
        door2.SetActive(false);
    }
    void OnTriggerEnter(Collider other)
    {
        planks.SetActive(true);
        hunter.SetActive(true);
        _newDoor1.SetActive(false);
        _newDoor2.SetActive(false);
        door1.SetActive(true);
        door2.SetActive(true);
        playerSource.clip = music;
        playerSource.Play();
        Destroy(gameObject);
    }
}
