using UnityEngine;
using TMPro;

public class Monologue_Intro : MonoBehaviour
{
    public TextMeshProUGUI monologueText;
    public void Phrase1()
    {
        monologueText.text = "Monday, late evening, around 21:30. It was a great summer, except for one thing...";
    }

    public void Phrase2()
    {
        monologueText.text = "My name is James Goodwill and I am a well-known reporter. I was especially known for my crime reporting.";
    }

    public void Phrase3()
    {
        monologueText.text = "That day I was driving to the small town - Pesinvill to do my report on a serial killer who had been operating there for a long time...";
    }

    public void Phrase4()
    {
        monologueText.text = "Police couldn't solve this case for a years, buy little I knew that I will be a cause of solving it. Moreover, I've solve it myself, and the police only had to take legal measures.";
    }

    public void Phrase5()
    {
        monologueText.text = "However, I would prefer that this did not happen in my life";
    }

    public void Phrase6()
    {
        monologueText.text = "But back to the topic. I'd been driving for about five hours, it was getting dark, and the nearest motel was a hundred miles away. I needed to stop at a gas station. Luckily, I'd planned everything and arrived there with only a few dozen miles of gas left.";
    }
}
