using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AudioManager : MonoBehaviour {

    public static AudioManager instance;

    public AudioSource[] efxSource;
    public AudioSource[] musicSource;

    public SoundGroup[] soundGroups;
    private Dictionary<string, AudioClip[]> groupDictionary = new Dictionary<string, AudioClip[]>();

    private string sceneName;

    private AudioSource sfxSource;
    private AudioSource[] musicSources;
    private int activeMusciSourceIndex;

    private float sfxVolumePercent;
    private float musicVolumePercent;


    private void Awake(){
        if (instance == null){
            instance = this;
        }
        else if (instance != null){
            Destroy(gameObject);
        }

        foreach (SoundGroup soundGroup in soundGroups){
            groupDictionary.Add(soundGroup.GroupID, soundGroup.group);
        }

        DontDestroyOnLoad(this);

    }

    private void Start(){
        OnLevelWasLoaded(0);

    }


    public void PlaySingle(AudioClip clip, bool playAlways){
        if (playAlways){
            efxSource[0].clip = clip;
            efxSource[0].Play();
            return;
        }

        if (efxSource[1].isPlaying == false){
            efxSource[1].clip = clip;
            efxSource[1].Play();
        }
        else{
            efxSource[2].clip = clip;
            efxSource[2].Play();
        }

    }

    public void StopSingle(AudioClip clip){
        if (efxSource[0].isPlaying == true && efxSource[0].clip == clip){
            efxSource[0].Stop();
        }
        else{
            efxSource[1].Stop();
        }

    }

    private void OnLevelWasLoaded(int level){
        string newSceneName = SceneManager.GetActiveScene().name;
        if (newSceneName != sceneName){
            sceneName = newSceneName;
            Invoke("PlayMusic", 0.2f);
        }

    }

    private void PlayMusic(){
        AudioClip clipToPlay = null;
        if (sceneName == "Menu"){
            clipToPlay = GetClipFromName("ThemeSong", 0);
        }
        else if (sceneName == "game"){
            clipToPlay = GetClipFromName("ThemeSong", 1);
        }

        if (clipToPlay != null){
            PlayMusic(clipToPlay, 2);
            Invoke("PlayMusic", clipToPlay.length);
        }

    }

    public void PlayMusic(AudioClip clip, float fadeDuration = 1f){
        activeMusciSourceIndex = 1 - activeMusciSourceIndex;
        musicSource[activeMusciSourceIndex].clip = clip;
        musicSource[activeMusciSourceIndex].Play();
        StartCoroutine(AnimateMusicCrossFade(fadeDuration));

    }

    private IEnumerator AnimateMusicCrossFade(float duration = 1){
        float percent = 0;

        while(percent < 1){
            percent += Time.deltaTime * 1 / duration;
        musicSource[activeMusciSourceIndex].volume = Mathf.Lerp(0, 1, percent);
        musicSource[1-activeMusciSourceIndex].volume = Mathf.Lerp(1, 0, percent);
            yield return null;
        }

    }


    public AudioClip GetRandomClipFromName(string name){
        if (groupDictionary.ContainsKey(name)){
            AudioClip[] sounds = groupDictionary[name];
            return sounds[Random.Range(0, sounds.Length)];
        }

        return null;
    }

    public AudioClip GetClipFromName(string name, int index){
        if (groupDictionary.ContainsKey(name)){
            AudioClip[] sounds = groupDictionary[name];
            return sounds[index];
        }

        return null;
    }

}

[System.Serializable]
public class SoundGroup{
    public string GroupID;
    public AudioClip[] group;

}
