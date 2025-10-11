namespace _Project.Scripts.Features.Base.Engine
{
    public interface IFeatureEngine
    {
        void Initialize();
        void Update();
        void FixedUpdate();
        void Dispose();
    }
}