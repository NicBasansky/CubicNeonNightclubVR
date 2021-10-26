using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class HologramManager : MonoBehaviour
{
    [SerializeField] float minIntervalSeconds = 30f;
    [SerializeField] float maxIntervalSeconds = 120f;
    //[SerializeField] HoloFish holoFish;
    [SerializeField] Transform spawnedFish;
    [SerializeField] bool playOnStart = false;
    [SerializeField] FlockingManager flockingManager;
    float interval;
    PlayableDirector playableDirector;

    void Awake()
    {
        playableDirector = GetComponentInChildren<PlayableDirector>();
    }

    void Start()
    {
        //holoFish.hologramManager = this;
        interval = GetRandomInterval();

        if (playOnStart)
        {
            spawnedFish.gameObject.SetActive(true);
            //holoFish.gameObject.SetActive(true);
            //flockingManager.SetShouldMove(true);
        }
        else
        {
            playableDirector.Stop();
            spawnedFish.gameObject.SetActive(false);
            StartCoroutine(PauseAndReplay());
            //holoFish.gameObject.SetActive(false);
        }
    }


    public void RestartTimer()
    {
        if (gameObject.activeSelf) // to prevent a bug starting the coroutine exiting play mode
            StartCoroutine(PauseAndReplay());
    }

    private float GetRandomInterval()
    {
        return UnityEngine.Random.Range(minIntervalSeconds, maxIntervalSeconds);
    }

    private IEnumerator PauseAndReplay()
    {
        yield return new WaitForSeconds(interval);

        spawnedFish.gameObject.SetActive(true);
        //holoFish.gameObject.SetActive(true);
        interval = GetRandomInterval();
        playableDirector.Play();
    }
}
