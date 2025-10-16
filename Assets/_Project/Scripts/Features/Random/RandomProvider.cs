using UnityEngine;

namespace _Project.Scripts.Features.Random
{
    public class RandomProvider : MonoBehaviour
    {
        public int seed;
        
        private System.Random _random;

        private void Awake()
        {
            UpdateSeed(seed);
        }

        public void UpdateSeed(int newSeed)
        {
            _random = new System.Random(newSeed);
        }

        public float Int()
        {
            return _random.Next();
        }

        public float Float()
        {
            return (float)_random.NextDouble();
        }

        public float InRange(float min, float max)
        {
            return (float)(_random.NextDouble() * (max - min) + min);
        }

        public int InRange(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}