using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.Features.Random
{
    public class RandomProvider : MonoBehaviour
    {
        public int seed;
        
        [Serializable]
        public struct RandomRotationAxis
        {
            public RandomRotationAxis(bool x, bool y, bool z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
            
            public bool x;
            public bool y;
            public bool z;
        }
        
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

        public int Index<T>(ICollection<T> collection)
        {
            if (collection == null || collection.Count == 0) return -1;
        
            return _random.Next(0, collection.Count);
        }

        public T Item<T>(ICollection<T> collection)
        {
            var randIndex = Index(collection);
            return randIndex == -1 ? default : collection.ElementAt(randIndex);
        }
        
        public Vector3 Rotation(RandomRotationAxis axis)
        {
            float x = axis.x ? _random.Next(0, 360) : 0;
            float y = axis.y ? _random.Next(0, 360) : 0;
            float z = axis.z ? _random.Next(0, 360) : 0;
    
            return new Vector3(x, y, z);
        }

        public Quaternion Quaternion(RandomRotationAxis axis)
        {
            var rotation = Rotation(axis);
            return UnityEngine.Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        }
    }
}