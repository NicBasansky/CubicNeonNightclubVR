using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO Add sound
public class SmokeMachine : MonoBehaviour
{
    [SerializeField] ParticleSystem spraySmokeParticles;
    [SerializeField] ParticleSystem roomSmokeParticles;
    float sprayDuration;
    float roomSmokeDuration;
    float roomSmokeEmissionDelay;
    
    ParticleSystem.EmissionModule sprayEmissionModule;
    ParticleSystem.EmissionModule roomSmokeEmissionModule;

    void Start()
    {
        sprayEmissionModule = spraySmokeParticles.emission;
        sprayEmissionModule.enabled = false;

        roomSmokeEmissionModule = roomSmokeParticles.emission;
        roomSmokeEmissionModule.enabled = false;
    }

    public void SetUp(LightshowManager.SmokeMachineConfig smokeMachineConfig)
    {
        sprayDuration = smokeMachineConfig.sprayDuration;
        roomSmokeDuration = smokeMachineConfig.roomSmokeDuration;
        roomSmokeEmissionDelay = smokeMachineConfig.roomSmokeEmissionDelay;
    }

    public void RunSmokeMachine()
    {
        StartCoroutine(RunSmokeMachineCoroutine());
    }

    private IEnumerator RunSmokeMachineCoroutine()
    {
        // start spraying
        sprayEmissionModule.enabled = true;

        yield return new WaitForSeconds(roomSmokeEmissionDelay);

        // after a few seconds, start emitting room smoke
        roomSmokeEmissionModule.enabled = true;

        yield return new WaitForSeconds(sprayDuration - roomSmokeEmissionDelay);

        // stop spraying smoke, let room smoke linger
        sprayEmissionModule.enabled = false;

        yield return new WaitForSeconds(roomSmokeDuration);

        // stop room smoke
        roomSmokeEmissionModule.enabled = false;

    }
   
}
