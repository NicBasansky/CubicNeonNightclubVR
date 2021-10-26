using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sparkler : MonoBehaviour
{
    [SerializeField] float minDistance = 10f;
    [SerializeField] float minIntervalSeconds = 120f;
    [SerializeField] float maxIntervalSeconds = 300f;
    [SerializeField] float minDuration = 15f;
    [SerializeField] float maxDuration = 25f;

    [SerializeField] ParticleSystem particleSystem_1;
    [SerializeField] ParticleSystem particleSystem_2;
    
    ParticleSystem.EmissionModule particleEmission_1;
    ParticleSystem.EmissionModule particleEmission_2;

    Transform player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        particleEmission_1 = particleSystem_1.emission;
        particleEmission_2 = particleSystem_2.emission;
    }

    void Start()
    {
        particleEmission_1.enabled = false;
        particleEmission_2.enabled = false;
        StartCoroutine(IgniteSparkler());
    }

    private IEnumerator IgniteSparkler()
    {
        while(true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minIntervalSeconds, maxIntervalSeconds));

            if (Vector3.Distance(player.position, transform.position) > minDistance)
                yield return null;
                
            particleEmission_1.enabled = true;
            particleEmission_2.enabled = true;

            yield return new WaitForSeconds(UnityEngine.Random.Range(minDuration, maxDuration));

            particleEmission_1.enabled = false;
            particleEmission_2.enabled = false;
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
