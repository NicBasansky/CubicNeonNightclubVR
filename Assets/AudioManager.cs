using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is for coordinating all of the music speakers that will be playing the same clip.
public class AudioManager : MonoBehaviour
{
    public AudioSource[] audioSources;
    //[SerializeField] float pauseGapSeconds = 2.0f;
    List<AudioClip> audioClips = new List<AudioClip>();
    public AudioSource musicAudioSource;
    public KeyCode muteKey = KeyCode.M;
    //public AudioSource movingAudioSource;
    //public AudioClip clip;
    int currentIndex = 0;


    void Update() // TODO remove
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayNextTrack();
            //LoadAudioSourceWithClip();
        }
        else if (Input.GetKeyDown(muteKey))
        {
            musicAudioSource.mute = !musicAudioSource.mute;
        }
    }

    public void PlayNextTrack()
    {
        currentIndex = (currentIndex + 1);
        if (currentIndex >= audioClips.Count)
        {
            currentIndex = 0;
        }

        //SetAudioClipToAllSources();
        SetAudioClipToMasterMusicAudioSource();
    }

    public void PlayPreviousTrack()
    {
        currentIndex = (currentIndex - 1);
        if (currentIndex < 0)
        {
            currentIndex = audioClips.Count - 1;
        }

        //SetAudioClipToAllSources();
        SetAudioClipToMasterMusicAudioSource();
    }

    public void SetAudioClips(List<AudioClip> clips)
    {
        audioClips = clips;
        //SetAudioClipToAllSources();
        //SetClipToMovingAudioSource();
        SetAudioClipToMasterMusicAudioSource();

    }

    private void SetAudioClipToMasterMusicAudioSource()
    {
        if (musicAudioSource.clip != null)
        {
            musicAudioSource.Stop();
            musicAudioSource.clip = null;
        }
        musicAudioSource.clip = audioClips[currentIndex];
        musicAudioSource.Play();
    
        StartCoroutine(PlayNextTrackAfterClipFinishes(audioClips[currentIndex]));
    }

    // private void SetClipToMovingAudioSource()
    // {
    //     if (movingAudioSource.clip != null)
    //     {
    //         movingAudioSource.Stop();
    //         movingAudioSource.clip = null;
    //     }
    //     movingAudioSource.clip = audioClips[currentIndex];
    //     movingAudioSource.Play();
    //     StartCoroutine(PlayNextTrackAfterClipFinishes(audioClips[currentIndex]));
    //}

    private void SetAudioClipToAllSources()
    {
        foreach(AudioSource source in audioSources)
        {
            if (source.clip != null)
            {
                source.Stop();
                source.clip = null;
            }
            source.clip = audioClips[currentIndex];
            source.Play();
        }
        StartCoroutine(PlayNextTrackAfterClipFinishes(audioClips[currentIndex]));
    }

    IEnumerator PlayNextTrackAfterClipFinishes(AudioClip currentClip)
    {

        yield return new WaitForSeconds(currentClip.length);
        PlayNextTrack();

    }
}
