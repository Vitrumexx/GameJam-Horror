using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Features.Random;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Features.Spawners
{
    public class RecycleObjectSpawner : MonoBehaviour
    {
        [Header("Dependencies")]
        public Transform pointsContainer;
        public GameObject prefab;
        [InfoBox("Можно не указывать")]
        public Transform spawnTo = null;
        
        [Header("Config")]
        [Min(0)] public int maxCount;
        [MinValue(0), MaxValue("maxCount")] public int minCount;
        [Min(0)] public float spawnDelay = 0f;
        public bool isFirstSpawnDelayed = false;
        public float offsetDistance = 0.2f;
        public SpawnRotationVariant spawnRotationVariant = SpawnRotationVariant.AsPrefab;
        public RandomProvider.RandomRotationAxis randomRotationAxis = new(false, false, true);
        
        private Transform[] _points;
        private readonly HashSet<GameObject> _spawnedObjects = new();
        private RandomProvider _randomProvider;
        private float _timeFromLastSpawn = 0f;

        public enum SpawnRotationVariant
        {
            AsPrefab = 0,
            Random = 1,
            AsPoint = 2
        }

        private void Start()
        {
            _timeFromLastSpawn = spawnDelay;
            
            _randomProvider = FindAnyObjectByType<RandomProvider>();
            _points = pointsContainer.GetComponentsInChildren<Transform>();
            
            HandleFirstSpawn();
        }

        private void HandleFirstSpawn()
        {
            if (isFirstSpawnDelayed) return;

            for (var i = 0; i < maxCount; i++)
            {
                TrySpawn();
            }
            
            _timeFromLastSpawn = 0f;
        }

        public void Update()
        {
            _spawnedObjects.RemoveWhere(x => x is null);

            if (_timeFromLastSpawn < spawnDelay)
            {
                _timeFromLastSpawn += Time.deltaTime;
                return;
            }

            if (_spawnedObjects.Count == maxCount)
            {
                return;
            }

            if (TrySpawn())
            {
                _timeFromLastSpawn = 0f;
            }
        }

        private bool TrySpawn()
        {
            if (!TryFindTransformToSpawn(new HashSet<Transform>(_points), out var randPoint))
            {
                return false;
            }
            
            var obj = Instantiate(prefab, spawnTo, true);
            obj.transform.position = randPoint.position;

            SetRotation(obj.transform, randPoint);
            
            _spawnedObjects.Add(obj);

            return true;
        }

        private void SetRotation(Transform objTransform, Transform point)
        {
            switch (spawnRotationVariant)
            {
                case SpawnRotationVariant.AsPoint:
                {
                    objTransform.localRotation = point.localRotation;
                    break;
                }
                case SpawnRotationVariant.Random:
                {
                    objTransform.localRotation = _randomProvider.Quaternion(randomRotationAxis);
                    break;
                }
                case SpawnRotationVariant.AsPrefab:
                default:
                {
                    break;
                }
            }
        }

        private bool TryFindTransformToSpawn(HashSet<Transform> points, out Transform randPoint)
        {
            while (true)
            {
                randPoint = _randomProvider.Item(points);

                if (randPoint is null) return false;

                if (IsPointClear(randPoint)) return true;

                points.Remove(randPoint);
            }
        }

        private bool IsPointClear(Transform point)
        {
            return _spawnedObjects.All(obj => !(Vector3.Distance(obj.transform.position, point.position) <= offsetDistance));
        }
    }
}