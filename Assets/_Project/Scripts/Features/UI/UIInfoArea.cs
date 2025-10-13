using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Features.UI
{
    [Serializable]
    public struct UIInfoArea
    {
        public CanvasGroup infoArea;
        public TextMeshProUGUI text;
        public Image icon;
    }
}