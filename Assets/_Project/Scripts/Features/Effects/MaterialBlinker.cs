using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Features.Effects
{
    public class MaterialBlinker : MonoBehaviour
    {
        private readonly string _colorProperty = "_Color";
        private readonly Dictionary<MeshRenderer, BlinkedMaterial> _meshRenderers = new();

        public struct BlinkedMaterial
        {
            public Material BlinkMaterial;
            public Material BaseMaterial;
            public Coroutine BlinkRoutine;
        }

        public void StartBlink(MeshRenderer meshRenderer, Color blinkColor, float duration)
        {
            if (meshRenderer == null) return;

            if (_meshRenderers.TryGetValue(meshRenderer, out var data))
            {
                if (data.BlinkRoutine != null)
                    StopCoroutine(data.BlinkRoutine);
                if (data.BlinkMaterial != null)
                    Destroy(data.BlinkMaterial);
            }

            var baseMaterial = meshRenderer.sharedMaterial;
            var blinkMaterial = new Material(baseMaterial);
            meshRenderer.material = blinkMaterial;

            var originalColor = blinkMaterial.GetColor(_colorProperty);
            var routine = StartCoroutine(BlinkRoutine(blinkMaterial, originalColor, blinkColor, duration));

            _meshRenderers[meshRenderer] = new BlinkedMaterial
            {
                BlinkMaterial = blinkMaterial,
                BaseMaterial = baseMaterial,
                BlinkRoutine = routine
            };
        }

        public void StopBlink(MeshRenderer meshRenderer)
        {
            if (meshRenderer == null) return;
            if (!_meshRenderers.TryGetValue(meshRenderer, out var data)) return;

            if (data.BlinkRoutine != null)
                StopCoroutine(data.BlinkRoutine);

            if (meshRenderer != null)
                meshRenderer.material = data.BaseMaterial;

            if (data.BlinkMaterial != null)
                Destroy(data.BlinkMaterial);

            _meshRenderers.Remove(meshRenderer);
        }

        private IEnumerator BlinkRoutine(Material material, Color original, Color blinkColor, float duration)
        {
            float half = duration * 0.5f;

            while (true)
            {
                yield return LerpColor(material, original, blinkColor, half);
                yield return LerpColor(material, blinkColor, original, half);
            }
        }

        private IEnumerator LerpColor(Material mat, Color from, Color to, float time)
        {
            float t = 0f;
            while (t < time)
            {
                t += Time.deltaTime;
                float lerp = t / time;
                mat.SetColor(_colorProperty, Color.Lerp(from, to, lerp));
                yield return null;
            }
        }

        private void OnDestroy()
        {
            foreach (var kvp in _meshRenderers)
            {
                var data = kvp.Value;
                if (data.BlinkRoutine != null)
                    StopCoroutine(data.BlinkRoutine);
                if (data.BlinkMaterial != null)
                    Destroy(data.BlinkMaterial);
                if (kvp.Key != null)
                    kvp.Key.material = data.BaseMaterial;
            }
            _meshRenderers.Clear();
        }
    }
}
