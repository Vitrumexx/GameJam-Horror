using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Mansion_Monologue : MonoBehaviour
{
    public TextMeshProUGUI monologueText;
    public void Phrase1()
    {
        monologueText.text = "";
    }

    public void Phrase2()
    {
        monologueText.text = "I had been driving for two hours from the gas station when suddenly my engine started making a strange sound...";
    }

    public void Phrase3()
    {
        monologueText.text = "So I found myself in the middle of the night in a dense forest, completely alone... Or not...";
    }

    public void Phrase4()
    {
        monologueText.text = "On the path that branched off from the road, I noticed road signs. I followed them.";
    }

    public void Phrase5()
    {
        monologueText.text = "I saw a luxurious mansion on the hill. A little panic and stress set in, so I went inside without thinking...";
    }

    public void Phrase6()
    {
        monologueText.text = "But back to the topic. I'd been driving for about five hours, it was getting dark, and the nearest motel was a hundred miles away. I needed to stop at a gas station. Luckily, I'd planned everything and arrived there with only a few dozen miles of gas left.";
    }
}
