using _Project.Scripts.Features.Shared;
using UnityEngine;

namespace _Project.Scripts.Features.Sounds
{
    public class SoundsStorage : MonoStorage<SoundStorableUnit>
    {
        public bool TryGetSoundStorableUnit(string soundId, out SoundStorableUnit storableUnit)
        {
            if (!Units.TryGetValue(soundId, out storableUnit))
            {
                Debug.Log($"Sound ID {soundId} not found!");
                return false;
            }
            
            return true;
        }
    }
}