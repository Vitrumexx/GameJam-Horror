using System.Linq;
using UnityEngine;

namespace _Project.Scripts.Features.Finders.FieldOfView
{
    public class VisibleTarget : MonoBehaviour {
        public Transform visibilityPointsContainer;

        public Transform[] VisibilityPoints { get; private set; }

        private VisibleTargetsRegistrator _visibleTargetsRegistrator;
        
        private void Start()
        {
            VisibilityPoints = visibilityPointsContainer
                .GetComponentsInChildren<Transform>();
            
            _visibleTargetsRegistrator = FindAnyObjectByType<VisibleTargetsRegistrator>();
            _visibleTargetsRegistrator?.RegisterItem(this);
        }

        private void OnDestroy()
        {
            _visibleTargetsRegistrator?.UnregisterItem(this);
        }
    }
}