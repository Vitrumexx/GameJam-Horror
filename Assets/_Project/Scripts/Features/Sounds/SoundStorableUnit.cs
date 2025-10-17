using _Project.Scripts.Features.Shared;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Features.Sounds
{
    public class SoundStorableUnit : StorableUnit
    {
        public AudioClip clip;
        
        [PropertyRange(0f, 1f)]
        public float volume = 1.0f;
    }
}