using System.Collections.Generic;
using _Project.Scripts.Features.UI;
using UnityEngine;

namespace _Project.Scripts.Features.Player
{
    public class PlayerOverlay : MonoBehaviour
    {
        public Transform overlay;
        public GameObject uiInfoAreaPrefab;

        private readonly Dictionary<string, UIInfoArea> _data = new();
        
        public void AddData(string overlayTag, string text, Sprite sprite = null)
        {
            if (_data.ContainsKey(overlayTag)) return;
            
            var obj = Instantiate(uiInfoAreaPrefab, overlay);
            var infoArea = obj.GetComponent<UIInfoArea>();

            infoArea.text.text = text;
            infoArea.SetIcon(sprite);

            if (!_data.TryAdd(overlayTag, infoArea))
            {
                Destroy(obj);
            }
        }

        public void RemoveData(string overlayTag)
        {
            if (!_data.Remove(overlayTag, out var uiInfoArea)) return;

            Destroy(uiInfoArea.gameObject);
        }

        public void UpdateData(string overlayTag, string text, Sprite sprite = null)
        {
            RemoveData(overlayTag);
            AddData(overlayTag, text, sprite);
        }
    }
}