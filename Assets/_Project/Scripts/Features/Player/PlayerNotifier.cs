using System.Collections;
using _Project.Scripts.Features.UI;
using UnityEngine;

namespace _Project.Scripts.Features.Player
{
    public class PlayerNotifier : MonoBehaviour
    {
        [Header("Notification Settings")]
        [SerializeField] private float notificationDuration = 2f;
        [SerializeField] private UIInfoArea infoArea;

        [Header("Bomb Animation Settings")]
        [SerializeField] private float appearDuration = 0.3f;
        [SerializeField] private float disappearDuration = 0.25f;
        [SerializeField] private float bombScaleMultiplier = 1.5f;
        [SerializeField] private AnimationCurve bombCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private float shakeIntensity = 10f;
        [SerializeField] private float shakeFrequency = 25f;

        private Coroutine _currentCoroutine;
        private Vector3 _originalScale;
        private RectTransform _rect;

        private void Awake()
        {
            _rect = infoArea.GetComponent<RectTransform>();
            _originalScale = _rect.localScale;
            infoArea.gameObject.SetActive(false);
        }

        public void ShowMessage(string msg, Sprite sprite = null)
        {
            infoArea.text.text = msg;
            if (sprite == null)
            {
                infoArea.icon.gameObject.SetActive(false);
            }
            else
            {
                infoArea.icon.sprite = sprite;
                infoArea.icon.gameObject.SetActive(true);
            }
        }

        public void NotifyPlayer(float duration, string msg, Sprite sprite = null)
        {
            if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);
            _currentCoroutine = StartCoroutine(NotifySequence(duration, msg, sprite));
        }

        public void NotifyPlayer(string msg, Sprite sprite = null)
        {
            NotifyPlayer(notificationDuration, msg, sprite);
        }

        private IEnumerator NotifySequence(float duration, string msg, Sprite sprite)
        {
            ShowMessage(msg, sprite);
            infoArea.gameObject.SetActive(true);
            yield return StartCoroutine(AnimateBombEffect(appearDuration, true));
            yield return new WaitForSeconds(duration);
            yield return StartCoroutine(AnimateBombEffect(disappearDuration, false));
            infoArea.gameObject.SetActive(false);
            _currentCoroutine = null;
        }

        private IEnumerator AnimateBombEffect(float time, bool appearing)
        {
            float elapsed = 0f;
            Vector3 startScale = appearing ? Vector3.zero : _originalScale;
            Vector3 endScale = appearing ? _originalScale * bombScaleMultiplier : Vector3.zero;
            Vector3 shakeOffset = Vector3.zero;

            while (elapsed < time)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / time;
                float eval = bombCurve.Evaluate(t);
                _rect.localScale = Vector3.Lerp(startScale, endScale, eval);

                if (shakeIntensity > 0f)
                {
                    float shake = Mathf.Sin(elapsed * shakeFrequency) * shakeIntensity * (1 - t);
                    shakeOffset.x = Random.Range(-shake, shake);
                    shakeOffset.y = Random.Range(-shake, shake);
                    _rect.localPosition = shakeOffset;
                }

                yield return null;
            }

            _rect.localScale = appearing ? _originalScale : Vector3.zero;
            _rect.localPosition = Vector3.zero;
        }
    }
}
