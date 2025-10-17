using UnityEngine;

namespace _Project.Scripts.Features.Finders.FieldOfView
{
    public class FieldOfViewFinder : MonoBehaviour
    {
        [Header("View Settings")]
        [SerializeField] private float viewRadius = 10f;
        [SerializeField, Range(0, 360)] private float viewAngle = 90f;
        [SerializeField] private Axis viewDirectionAxis = Axis.Z;
        [SerializeField] private bool invertAxis = false;

        [Header("Layers")]
        [SerializeField] private LayerMask obstacleMask;

        public bool CanSeeTarget { get; private set; }
        public VisibleTarget VisibleTarget { get; private set; }
        
        private VisibleTargetsRegistrator _visibleTargetsRegistrator;
        private Vector3 _closestVisiblePoint;

        private enum Axis { X, Y, Z }

        private void Start()
        {
            _visibleTargetsRegistrator = FindAnyObjectByType<VisibleTargetsRegistrator>();
        }

        private void Update()
        {
            FindVisibleTarget();
        }

        private Vector3 ViewDirection
        {
            get
            {
                Vector3 dir = viewDirectionAxis switch
                {
                    Axis.X => transform.right,
                    Axis.Y => transform.up,
                    _ => transform.forward
                };
                return invertAxis ? -dir : dir;
            }
        }

        private void FindVisibleTarget()
        {
            CanSeeTarget = false;
            VisibleTarget = null;

            if (_visibleTargetsRegistrator is null || _visibleTargetsRegistrator.Units.Count == 0) return;

            foreach (var target in _visibleTargetsRegistrator.Units)
            {
                if (target == null) continue;

                bool targetVisible = false;
                Vector3 closestVisiblePoint = Vector3.zero;
                float minDistance = float.MaxValue;

                foreach (var point in target.VisibilityPoints)
                {
                    var toPoint = point.position - transform.position;
                    var distanceToPoint = toPoint.magnitude;

                    if (distanceToPoint > viewRadius) continue;

                    var angleToPoint = Vector3.Angle(ViewDirection, toPoint);
                    if (angleToPoint > viewAngle / 2f) continue;

                    var dir = toPoint.normalized;
                    if (!Physics.Raycast(transform.position, dir, distanceToPoint, obstacleMask))
                    {
                        Debug.DrawRay(transform.position, dir * distanceToPoint, Color.green);
                        if (distanceToPoint < minDistance)
                        {
                            minDistance = distanceToPoint;
                            closestVisiblePoint = point.position;
                            targetVisible = true;
                        }
                    }
                    else
                    {
                        Debug.DrawRay(transform.position, dir * distanceToPoint, Color.red);
                    }
                }

                if (targetVisible)
                {
                    CanSeeTarget = true;
                    VisibleTarget = target;
                    _closestVisiblePoint = closestVisiblePoint;
                    return;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, viewRadius);

            var leftBoundary = DirFromAngle(-viewAngle / 2, true);
            var rightBoundary = DirFromAngle(viewAngle / 2, true);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewRadius);
            Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewRadius);

            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(transform.position, ViewDirection * viewRadius);

            if (!CanSeeTarget || VisibleTarget is null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _closestVisiblePoint);
        }

        private Vector3 DirFromAngle(float angleInDegrees, bool isGlobal)
        {
            if (!isGlobal) angleInDegrees += transform.eulerAngles.y;
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}