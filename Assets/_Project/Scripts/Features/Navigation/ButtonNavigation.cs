using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Features.Navigation
{
    public class ButtonNavigation : MonoBehaviour
    {
        public void LoadScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }

        public void LoadScene(int scene)
        {
            SceneManager.LoadScene(scene);
        }

        public void LoadMansion()
        {
            SceneManager.LoadScene("Mansion");
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene("Lobby");
        }
    }
}