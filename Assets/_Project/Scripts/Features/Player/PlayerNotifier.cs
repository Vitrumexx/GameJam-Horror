using System.Collections;
using _Project.Scripts.Features.Random;
using _Project.Scripts.Features.UI;
using UnityEngine;

namespace _Project.Scripts.Features.Player
{
    public class PlayerNotifier : MonoBehaviour
    {
        private enum AnimationMode { Bomb, Letters }

        [Header("Notification Settings")]
        [SerializeField] private float notificationDuration = 2f;
        [SerializeField] private UIInfoArea infoArea;

        [Header("Animation Mode")]
        [SerializeField] private AnimationMode animationMode = AnimationMode.Bomb;

        [Header("Bomb Animation Settings")]
        [SerializeField] private float appearDuration = 0.3f;
        [SerializeField] private float disappearDuration = 0.25f;
        [SerializeField] private float bombScaleMultiplier = 1.5f;
        [SerializeField] private AnimationCurve bombCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private float shakeIntensity = 10f;
        [SerializeField] private float shakeFrequency = 25f;

        [Header("Letter Animation Settings")]
        [SerializeField] private float letterDelay = 0.04f;

        private Coroutine _currentCoroutine;
        private Coroutine _textCoroutine;
        private Vector3 _originalScale;
        private RectTransform _rect;
        private RandomProvider _randomProvider;

        private void Awake()
        {
            _rect = infoArea.GetComponent<RectTransform>();
            _originalScale = _rect.localScale;
            infoArea.gameObject.SetActive(false);
        }

        private void Start()
        {
            _randomProvider = FindAnyObjectByType<RandomProvider>();
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
            infoArea.gameObject.SetActive(true);
            infoArea.icon.gameObject.SetActive(false);

            if (animationMode == AnimationMode.Bomb)
            {
                infoArea.text.text = msg;
                if (sprite != null)
                {
                    infoArea.icon.sprite = sprite;
                    infoArea.icon.gameObject.SetActive(true);
                }

                yield return StartCoroutine(AnimateBombEffect(appearDuration, true));
                yield return new WaitForSeconds(duration);
                yield return StartCoroutine(AnimateBombEffect(disappearDuration, false));
            }
            else
            {
                if (_textCoroutine != null) StopCoroutine(_textCoroutine);
                _textCoroutine = StartCoroutine(AnimateTextLetters(msg, true));

                yield return _textCoroutine;

                if (sprite != null)
                {
                    infoArea.icon.sprite = sprite;
                    infoArea.icon.gameObject.SetActive(true);
                }

                yield return new WaitForSeconds(duration);

                infoArea.icon.gameObject.SetActive(false);
                if (_textCoroutine != null) StopCoroutine(_textCoroutine);
                _textCoroutine = StartCoroutine(AnimateTextLetters(msg, false));
                yield return _textCoroutine;
            }

            infoArea.text.text = "";
            infoArea.gameObject.SetActive(false);
            _currentCoroutine = null;
            _textCoroutine = null;
        }

        private IEnumerator AnimateBombEffect(float time, bool appearing)
        {
            float elapsed = 0f;
            Vector3 startScale = appearing ? Vector3.zero : _originalScale;
            Vector3 endScale = appearing ? _originalScale * bombScaleMultiplier : Vector3.zero;
            Vector3 basePosition = _rect.localPosition;
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
                    shakeOffset.x = _randomProvider.InRange(-shake, shake);
                    shakeOffset.y = _randomProvider.InRange(-shake, shake);
                    _rect.localPosition = basePosition + shakeOffset;
                }

                yield return null;
            }

            _rect.localScale = appearing ? _originalScale : Vector3.zero;
            _rect.localPosition = basePosition;
        }

        private IEnumerator AnimateTextLetters(string msg, bool appearing)
        {
            infoArea.text.text = "";

            if (appearing)
            {
                for (int i = 0; i < msg.Length; i++)
                {
                    infoArea.text.text = msg.Substring(0, i + 1);
                    yield return new WaitForSeconds(letterDelay);
                }
            }
            else
            {
                for (int i = msg.Length; i >= 0; i--)
                {
                    infoArea.text.text = msg.Substring(0, i);
                    yield return new WaitForSeconds(letterDelay);
                }
            }
        }
    }
}
