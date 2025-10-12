namespace _Project.Scripts.Features.Base.Engine
{
    public interface IFeaturesEngine
    {
        void Initialize();
        void Update();
        void FixedUpdate();
        void Dispose();
    }
}