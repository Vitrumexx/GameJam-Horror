using System.Collections.Generic;
using _Project.Scripts.Features.UI;
using UnityEngine;

namespace _Project.Scripts.Features.Player
{
    public class PlayerOverlay : MonoBehaviour
    {
        public Transform overlay;
        public GameObject uiInfoAreaPrefab;

        private Dictionary<string, UIInfoArea> _data;
        
        private static readonly string InteractableKey = "interactable";
        
        public void ShowInteractable(KeyCode keyToInteract, Sprite sprite = null)
        {
            if (_data.ContainsKey(InteractableKey)) return;
            
            var obj = Instantiate(uiInfoAreaPrefab, overlay);
            var infoArea = obj.GetComponent<UIInfoArea>();

            infoArea.text.text = $"Click \"{keyToInteract}\" to interact.";
            infoArea.SetIcon(sprite);
        }

        public void HideInteractable()
        {
            if (!_data.TryGetValue(InteractableKey, out var uiInfoArea)) return;
            
            Destroy(uiInfoArea.gameObject);
        }
    }
}