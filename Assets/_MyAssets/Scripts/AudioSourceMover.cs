using System;
using System.Collections;
using UnityEngine;

public class AudioSourceMover : MonoBehaviour 
{

    [SerializeField] AudioSource mainAudioSource;
    [SerializeField] Transform[] speakerTransforms;
    [SerializeField] float checkInterval = 0.5f;
    GameObject player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        StartCoroutine(CheckClosestSpeaker());
    }

    IEnumerator CheckClosestSpeaker()
    {
        if (speakerTransforms.Length == 0) yield return null;

        while(true)
        {
            yield return new WaitForSeconds(checkInterval);
            print("checking for nearest speaker");
            Transform closestSpeaker = speakerTransforms[0];
            float closestDistance = Mathf.Infinity;
            
            for (int i = 0; i < speakerTransforms.Length; i++)
            {
                if (Vector3.Distance(player.transform.position, speakerTransforms[i].position) < closestDistance)
                {
                    closestDistance = Vector3.Distance(player.transform.position, speakerTransforms[i].position);
                    closestSpeaker = speakerTransforms[i];
                }
            }

            SetClosestSpeaker(closestSpeaker);
        }
    }

    private void SetClosestSpeaker(Transform closestSpeaker)
    {
        mainAudioSource.transform.parent = closestSpeaker;
        mainAudioSource.transform.position = closestSpeaker.position;
    }
}