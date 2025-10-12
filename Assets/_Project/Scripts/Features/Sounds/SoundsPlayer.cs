using UnityEngine;

namespace _Project.Scripts.Features.Sounds
{
    public class SoundsPlayer : MonoBehaviour
    {
        public SoundsStorage soundsStorage;
        public AudioSource defaultAudioSource;

        public void PlayClipOnDefault(AudioClip clip)
        {
            defaultAudioSource.PlayOneShot(defaultAudioSource.clip);
        }

        public void PlayClipOnDefault(AudioClip clip, float volume)
        {
            defaultAudioSource.volume = volume;
            PlayClipOnDefault(clip);
        }
        
        public void PlayClipOnDefault(string soundId)
        {
            if (!soundsStorage.TryGetSoundStorableUnit(soundId, out var sound))
            {
                return;
            }
            
            PlayClipOnDefault(sound.clip, sound.volume);
        }

        public void PlayClip(AudioSource audioSource, AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }

        public void PlayClip(AudioSource audioSource, AudioClip clip, float volume)
        {
            audioSource.volume = volume;
            PlayClip(audioSource, clip);
        }

        public void PlayClip(AudioSource audioSource, string soundId)
        {
            if (!soundsStorage.TryGetSoundStorableUnit(soundId, out var storableUnit))
            {
                return;
            }
            
            PlayClip(audioSource, storableUnit.clip, storableUnit.volume);
        }
    }
}