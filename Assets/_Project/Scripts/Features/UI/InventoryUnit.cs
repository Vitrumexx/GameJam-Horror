using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Features.UI
{
    public class InventoryUnit : MonoBehaviour
    {
        public Image icon;
        public Image deselectedFrame;
        public Image selectedFrame;

        private void Start()
        {
            Clear();
        }

        public void Select()
        {
            selectedFrame.gameObject.SetActive(true);
            deselectedFrame.gameObject.SetActive(false);
        }

        public void Deselect()
        {
            selectedFrame.gameObject.SetActive(false);
            deselectedFrame.gameObject.SetActive(true);
        }

        public void SetIcon(Sprite sprite = null)
        {
            if (sprite is null)
            {
                icon.gameObject.SetActive(false);
                return;
            }
            
            icon.gameObject.SetActive(true);
            icon.sprite = sprite;
        }

        public virtual void Clear()
        {
            SetIcon(null);
        }
    }
}