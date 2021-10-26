using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO Add variation to the appearance of smoke with the fog machine
public class LightshowManager : MonoBehaviour
{
    [System.Serializable]
    public class SpotlightShowScene
    {
        public string sceneName;
        [Header("Pin Spots")]
        public Color lowPinSpotColor = Color.black;
        public Color highPinSpotColor = Color.black;
        [Header("Spotlights")]
        public Color centerSpotColor = Color.black;
        public Color sidesSpotColor = Color.black;
    }
    public enum LaserType
    {
        Green_l,
        Green_r,
        Pink,
        Blue,
        Purple
    };

    [System.Serializable]
    public struct PinSpot
    {
        public Light light;
        public MeshRenderer lightBeamMeshRenderer;
    }
    [System.Serializable]
    public struct LaserProjector
    {
        public LaserType laserType;
        public Animator animator;
    }
    [System.Serializable]
    public class LaserProjectorConfig
    {
        public LaserType laserType;
        public float speedMultiplier = 1f;
    }

    [System.Serializable]
    public class LaserShowScene
    {
        public string sceneName;
        public float minDuration;
        public float maxDuration;
        public LaserProjectorConfig[] laserProjectorConfigs;

    }

    [System.Serializable]
    public class SmokeMachineConfig
    {
        public SmokeMachine smokeMachine;
        public bool runOnStart = false;
        public float minInterval = 360f; // 6 mins
        public float maxInterval = 600f; // 10 mins
        public float sprayDuration = 15f;
        public float roomSmokeDuration = 240f; // 4 mins
        public float roomSmokeEmissionDelay = 5.5f;
    }

    [Header("Light References")]
    public Light[] sideSpotlights;
    public Light[] centreSpotlights;
    
    public PinSpot[] highPinSpots;
    public PinSpot[] lowPinSpots;

    public LaserProjector[] laserProjectors;

    [Header("Spotlights")]
    public bool randomizeScenes = true;
    public float spotlightSceneMinDuration = 5f;
    public float spotlightSceneMaxDuration = 120f;
    public SpotlightShowScene[] spotlightShowScenes;

    [Header("Laser Scenes")]
    public LaserShowScene[] laserShowScenes;

    [Header("Smoke Machine")]
    public SmokeMachineConfig smokeMachineConfig;

    [Header("For Scene Building")]
    public bool sceneBuilding = false;
    // public MeshRenderer renderer;
    public Color lowPinSpotColor;
    public Color highPinSpotColor;
    public Color centerSpotColor;
    public Color sidesSpotColor;

    int currentSpotlightSceneIndex = 0;
    int currentLaserSceneIndex = 0;

    void Start()
    {
     
        if (spotlightShowScenes.Length != 0)
        {
            PlaySpotlightShowScene(spotlightShowScenes[currentSpotlightSceneIndex]);
        }

        if (laserShowScenes.Length != 0)
        {
            PlayLaserShowScene(laserShowScenes[currentLaserSceneIndex]);
        }
        else
        {
            foreach (var laserProjector in laserProjectors)
            {
                laserProjector.animator.SetBool("isOn", false);
            }
        }

        if (smokeMachineConfig.smokeMachine != null && smokeMachineConfig.smokeMachine.isActiveAndEnabled)
        {
            smokeMachineConfig.smokeMachine.SetUp(smokeMachineConfig);
            StartCoroutine(RunSmokeMachine());
        }

        if (!sceneBuilding)
        {
            StartCoroutine(ChangeSpotlightScene());

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!sceneBuilding) return;
        //renderer.material.SetColor("_TintColor", color);
        foreach (PinSpot spot in lowPinSpots)
        {
            spot.light.color = lowPinSpotColor;
            spot.lightBeamMeshRenderer.material.SetColor("_TintColor", lowPinSpotColor);

        }
        foreach (PinSpot spot in highPinSpots)
        {
            spot.light.color = highPinSpotColor;
            spot.lightBeamMeshRenderer.material.SetColor("_TintColor", highPinSpotColor);

        }
        foreach (Light spot in centreSpotlights)
        {
            spot.color = centerSpotColor;
        }
        foreach (Light spot in sideSpotlights)
        {
            spot.color = sidesSpotColor;
        }

    }

    private void PlayLaserShowScene(LaserShowScene laserShowScene)
    {
        
        // set all lasers to off first
        foreach (var laserProjector in laserProjectors)
        {
            // if (laserProjector.animator == null)
            // {
            //     return; // trying to leave the scene, this prevents this from continuing to run
            // }
            laserProjector.animator.SetBool("isOn", false);
        }

        // rest scenes
        if (laserShowScene.laserProjectorConfigs.Length == 0 || 
                laserShowScene.sceneName == "RestScene")
        {
            StartCoroutine(LaserSceneTimerCo(laserShowScene));
            return;
        }
        
        foreach(var laserProjector in laserProjectors)
        {
            foreach(var config in laserShowScene.laserProjectorConfigs)
            {
                if (laserProjector.laserType == config.laserType)
                {
                    
                    laserProjector.animator.SetBool("isOn", true);
                    laserProjector.animator.SetFloat("speedMult",
                                UnityEngine.Random.Range(-1.1f, 1.1f));

                }
            }
        }

        StartCoroutine(LaserSceneTimerCo(laserShowScene));
    }

    private IEnumerator LaserSceneTimerCo(LaserShowScene currentLaserShowScene)
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(
            currentLaserShowScene.minDuration, currentLaserShowScene.maxDuration));

        currentLaserSceneIndex++;
        if (currentLaserSceneIndex >= laserShowScenes.Length)
        {          
            currentLaserSceneIndex = 0;
        }

        PlayLaserShowScene(laserShowScenes[currentLaserSceneIndex]);
    }

    private IEnumerator ChangeSpotlightScene()
    {
        while (spotlightShowScenes.Length > 1)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(
                            spotlightSceneMinDuration, spotlightSceneMaxDuration));

            if (randomizeScenes)
            {
                currentSpotlightSceneIndex = UnityEngine.
                                            Random.Range(0, spotlightShowScenes.Length);
            }
            else
            {
                currentSpotlightSceneIndex++;
                if (currentSpotlightSceneIndex >= spotlightShowScenes.Length)
                {
                    currentSpotlightSceneIndex = 0;
                }

            }

            PlaySpotlightShowScene(spotlightShowScenes[currentSpotlightSceneIndex]);
        }
    }

    void PlaySpotlightShowScene(SpotlightShowScene scene)
    {
        foreach (PinSpot spot in lowPinSpots)
        {
            spot.light.color = scene.lowPinSpotColor;
            spot.lightBeamMeshRenderer.material.SetColor("_TintColor", scene.lowPinSpotColor);
        }
        foreach (PinSpot spot in highPinSpots)
        {
            spot.light.color = scene.highPinSpotColor;
            spot.lightBeamMeshRenderer.material.SetColor("_TintColor", scene.highPinSpotColor);

        }
        foreach(Light spot in centreSpotlights)
        {
            spot.color = scene.centerSpotColor;
        }
        foreach(Light spot in sideSpotlights)
        {
            spot.color = scene.sidesSpotColor;
        }
    }

    private IEnumerator RunSmokeMachine()
    {
        if (smokeMachineConfig.runOnStart)
        {
            smokeMachineConfig.smokeMachine.RunSmokeMachine();
        }
        while(true)
        {
            // to help prevent overlap of running smoke coroutines over eachother
            float minInterval = smokeMachineConfig.minInterval + smokeMachineConfig.roomSmokeDuration;
            float maxInterval = smokeMachineConfig.maxInterval + smokeMachineConfig.roomSmokeDuration;
            yield return new WaitForSeconds(UnityEngine.Random.Range(minInterval, maxInterval));
            
            smokeMachineConfig.smokeMachine.RunSmokeMachine();
        }
    }
}
