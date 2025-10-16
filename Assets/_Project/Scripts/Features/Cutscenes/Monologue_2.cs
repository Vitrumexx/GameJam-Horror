using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Monologue_2 : MonoBehaviour
{
    public TextMeshProUGUI monologueText;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CutsceneSignals.Instance.StartCutscene("Monologue_2");
            Destroy(gameObject);
        }
    }
    public void Phrase1()
    {
        monologueText.text = "I've parked near a gas station and started filling up.";
    }

    public void Phrase2()
    {
        monologueText.text = "I went inside to buy myself some food for the road and pay for gas.";
    }

    public void Phrase3()
    {
        monologueText.text = "It was already dark and quite cold outside.";
    }

}
