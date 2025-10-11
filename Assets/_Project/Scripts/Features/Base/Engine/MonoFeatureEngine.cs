using System;
using _Project.Scripts.Features.Base.Interfaces;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Features.Base.Engine
{
    public class MonoFeatureEngine : MonoBehaviour, IFeatureEngine
    {
        public IInitializableFeature[] InitializableFeatures { get; private set; }
        public IUpdatableFeature[] UpdatableFeatures { get; private set; }
        public IFixedUpdatableFeature[] FixedUpdatableFeatures { get; private set; }
        public IDisposableFeature[] DisposableFeatures { get; private set; }

        [Inject]
        private void Construct(
            IInitializableFeature[] initializableFeatures,
            IUpdatableFeature[] updatableFeatures,
            IFixedUpdatableFeature[] fixedUpdatableFeatures,
            IDisposableFeature[] disposableFeatures)
        {
            InitializableFeatures = initializableFeatures;
            UpdatableFeatures = updatableFeatures;
            FixedUpdatableFeatures = fixedUpdatableFeatures;
            DisposableFeatures = disposableFeatures;
        }

        public void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            foreach(var feature in InitializableFeatures) feature.Initialize();
        }

        public void Update()
        {
            foreach(var feature in UpdatableFeatures) feature.Update(Time.deltaTime);
        }

        public void FixedUpdate()
        {
            foreach(var feature in FixedUpdatableFeatures) feature.FixedUpdate(Time.fixedDeltaTime);
        }

        public void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            foreach(var feature in DisposableFeatures) feature.Dispose();
        }
    }
}