using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Features.UI
{
    public class UIInfoArea : MonoBehaviour
    {
        public TextMeshProUGUI text;
        public Image icon;

        public void SetIcon(Sprite sprite = null)
        {
            if (sprite is null)
            {
                icon.gameObject.SetActive(false);
                return;
            }
            
            icon.sprite = sprite;
            icon.gameObject.SetActive(true);
        }
    }
}