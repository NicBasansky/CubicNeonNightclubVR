using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;

public class MusicLoader : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] float volume = 0.2f;
    AudioSource audioSource;
    AudioManager audioManager;
    string[] musicFilePaths;
    //private string musicFile = "Music\\Fisher - Ya Kidding (Original Mix) OGG";
    public List<AudioClip> musicClips = new List<AudioClip>();

    void Awake()
    {
        //audioSource = GetComponent<AudioSource>();
        audioManager = GetComponent<AudioManager>();
    }

    void Start()
    {

        //musicFilePaths = Directory.GetFiles(Application.streamingAssetsPath + "/Music/", "*.ogg");
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath + "/Music/");
        FileInfo[] fileInfos = directoryInfo.GetFiles("*.ogg");
        StartCoroutine(PopulateAudioClips(fileInfos));
        //StartCoroutine(MusicPlayer());
        // foreach(var file in fileInfos)
        // {
        //     print(file);
        // }
        


    }

    public static string GetFileLocation(string relativePath)
    {
        return "file://" + Path.Combine(Application.streamingAssetsPath + "/" + relativePath + ".ogg");
    }

    IEnumerator PopulateAudioClips(FileInfo[] musicFiles)
    {
        foreach(var musicFile in musicFiles)
        {
            if (musicFile.Name.Contains(".meta")) // example just has "meta"
            {
                yield break; // increase index counter?
            }
            using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip("file://" + musicFile.FullName, AudioType.OGGVORBIS))
            {
                yield return uwr.SendWebRequest();

                if (uwr.isNetworkError || uwr.isHttpError)
                {
                    Debug.Log(uwr.error);
                }
                else
                {
                    AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);
                    musicClips.Add(clip);
                }
            }

        }
        audioManager.SetAudioClips(musicClips);
        //LoadAudioSourceWithClip();

    }

    // private void LoadAudioSourceWithClip()
    // {
    //     if (musicClips[currentIndex] == null) return;

    //     if (audioSource == null)
    //     {
    //         audioSource = gameObject.AddComponent<AudioSource>();

    //     }
    //     else if (audioSource.clip != null)
    //     {
    //         audioSource.Stop();
    //         audioSource.clip = null;
    //     }

    //     audioSource.loop = false; // TODO remove, should be able to play next track
    //     audioSource.volume = volume;
        
    //     audioSource.clip = musicClips[currentIndex];

    //     audioSource.Play();
    // }

    // IEnumerator MusicPlayer()
    // {
    //     //using (UnityWebRequest unityWebRequest = UnityWebRequestMultimedia.GetAudioClip(GetFileLocation(musicFile), AudioType.OGGVORBIS))
    //     using (UnityWebRequest unityWebRequest = UnityWebRequestMultimedia.GetAudioClip("file://" + musicFilePaths[currentIndex], AudioType.OGGVORBIS))        
    //     {
    //         yield return unityWebRequest.SendWebRequest();

    //         if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
    //         {
    //             Debug.Log(unityWebRequest.error);
    //         }
    //         else
    //         {
    //             if (audioSource == null)
    //             {
    //                 audioSource = gameObject.AddComponent<AudioSource>();

    //             }
    //             else if (audioSource.clip != null)
    //             {
    //                 // unload the existing clip
    //                 audioSource.Stop();
    //                 AudioClip currentClip = audioSource.clip;
    //                 audioSource.clip = null;
    //                 currentClip.UnloadAudioData();
    //                 DestroyImmediate(currentClip, false);
    //             }

    //             audioSource.loop = false; // TODO remove, should be able to play next track
    //             audioSource.volume = volume;
    //             audioSource.clip = DownloadHandlerAudioClip.GetContent(unityWebRequest);

                

    //             audioSource.Play();
    //             yield return null;
    //         }

    //     }

    // }

    
}
