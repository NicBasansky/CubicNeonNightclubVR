using UnityEngine;

namespace NicLib.Audio.Utilities
{
    public class StartAudioAtRandomPoint : MonoBehaviour
    {
        AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            float totalClipSeconds = audioSource.clip.length;
            float randomTime = Random.Range(0, totalClipSeconds);
            audioSource.time = randomTime;
            audioSource.Play();
        }
    }

}
