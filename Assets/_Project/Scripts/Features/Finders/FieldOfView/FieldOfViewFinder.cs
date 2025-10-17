using UnityEngine;

namespace _Project.Scripts.Features.Finders.FieldOfView
{
    public class FieldOfViewFinder : MonoBehaviour {
        [Header("View Settings")]
        [SerializeField] private float viewRadius = 10f;
        [SerializeField, Range(0, 360)] private float viewAngle = 90f;

        [Header("Layers")]
        [SerializeField] private LayerMask obstacleMask;

        public bool CanSeeTarget { get; private set; }
        public VisibleTarget VisibleTarget { get; private set; }
        
        private VisibleTargetsRegistrator _visibleTargetsRegistrator;

        private void Start() {
            _visibleTargetsRegistrator = FindAnyObjectByType<VisibleTargetsRegistrator>();
        }

        private void Update() {
            FindVisibleTarget();
        }

        private void FindVisibleTarget() {
            CanSeeTarget = false;
            VisibleTarget = null;

            if (_visibleTargetsRegistrator is null || _visibleTargetsRegistrator.Units.Count == 0) return;

            foreach (var target in _visibleTargetsRegistrator.Units) {
                var toTarget = target.transform.position - transform.position;
                var distanceToTarget = toTarget.magnitude;
                if (distanceToTarget > viewRadius) continue;

                var angleToTarget = Vector3.Angle(transform.forward, toTarget);
                if (angleToTarget > viewAngle / 2f) continue;

                foreach (var point in target.VisibilityPoints) {
                    var dir = (point - transform.position).normalized;
                    var dist = Vector3.Distance(transform.position, point);

                    if (Physics.Raycast(transform.position, dir, dist, obstacleMask)) continue;

                    CanSeeTarget = true;
                    VisibleTarget = target;
                    return;
                }   
            }
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, viewRadius);

            var leftBoundary = DirFromAngle(-viewAngle / 2, true);
            var rightBoundary = DirFromAngle(viewAngle / 2, true);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewRadius);
            Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewRadius);

            if (!CanSeeTarget || VisibleTarget is null) return;
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, VisibleTarget.transform.position);
        }

        private Vector3 DirFromAngle(float angleInDegrees, bool isGlobal) {
            if (!isGlobal) angleInDegrees += transform.eulerAngles.y;
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}
