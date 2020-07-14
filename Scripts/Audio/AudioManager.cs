using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public AudioMixerGroup mixer;
    public AudioMixerGroup musicMixer;
    public AudioMixerGroup sfxMixer;
    public AudioMixerGroup voicesMixer;

    public static AudioManager instance;

    public string musicPlaying = "NotMusic";
    public bool musicIsPaused;
    public Dictionary<string, string> stageMusic = new Dictionary<string, string>();

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        musicPlaying = "NotYet";

        DontDestroyOnLoad(gameObject);

        Setup();
    }

    private void Setup()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.outputAudioMixerGroup = mixer;

            //beginning of each clips name indicates the type of audio it is
            if (s.name.Length > 5 && s.name.Substring(0, 5) == "Music")
            {
                s.source.outputAudioMixerGroup = musicMixer;
            }
            if (s.name.Length > 3 && s.name.Substring(0, 3) == "SFX")
            {
                s.source.outputAudioMixerGroup = sfxMixer;
            }
            if (s.name.Length > 5 && s.name.Substring(0, 5) == "Voice")
            {
                s.source.outputAudioMixerGroup = voicesMixer;
            }


            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop ;
        }
    }

    private void Start()
    {
        // for each scene, must add it's appropriate music to the stageMusic
        stageMusic.Add("Level", "MusicGrooveFiend");
        stageMusic.Add("MainMenu", "MusicDallas");
        stageMusic.Add("End", "MusicDeep");

        SceneManager.sceneLoaded += SceneLoaded;
        SceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void SceneLoaded(Scene s, LoadSceneMode m)
    {
        string music;
        // Find appropriate music for scene
        stageMusic.TryGetValue(s.name, out music);

        // stop whatever is currently playing unless they are the same song
        if (musicPlaying.Substring(0, 5) == "Music" && music != musicPlaying)
        {
            Stop(musicPlaying);
        }

        // Now, play music for the stage you have woken up in
        if (music != musicPlaying)
        {
            Play(music, 1);
        }
    }

    public void Play(string name, float pitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("sound " + name + " not found.");
            return;
        }

        s.source.pitch = pitch;

        /*if (PauseMenuUI.GameIsPaused)
        {
            s.source.pitch -= 0.2f;
        }*/

        if(name.Length > 5 && name.Substring(0, 5) == "Music")
        {
            musicPlaying = name;
        }


        if (!s.source.isPlaying)
        {
            s.source.Play();
        }
    }

    // allows playing from designated audiosource
    public void PlayFromSource(string name, float pitch, float volume, AudioSource audiosource)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("sound " + name + " not found.");
            return;
        }

        if (name.Length > 5 && name.Substring(0, 5) == "Music")
        {
            musicPlaying = name;
        }


        if (!s.source.isPlaying)
        {
            audiosource.clip = s.source.clip;
            audiosource.outputAudioMixerGroup = s.source.outputAudioMixerGroup;
            audiosource.volume = s.source.volume;
            audiosource.pitch = pitch;
            audiosource.loop = s.source.loop;
            audiosource.spatialBlend = 1f;
            audiosource.Play();
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("sound " + name + " not found.");
            return;
        }

        if (name.Length > 5 && name.Substring(0, 5) == "Music")
        {
            musicPlaying = name;
        }

        if (!s.source.isPlaying)
        {
            s.source.Play();
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    public void Pause(string name)
    {
        musicIsPaused = true;
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Pause();
    }

    public void PauseAll()
    {
        musicIsPaused = true;
        foreach (Sound s in sounds)
        {
            s.source.Pause();
        }
    }

    public void UnPause(string name)
    {
        musicIsPaused = false;
        Sound s = Array.Find(sounds, sound => sound.name == name);

        /*if (PauseMenuUI.GameIsPaused)
        {
            s.source.pitch -= 0.2f;
        }
        else
        {
            s.source.pitch = 1f;
        }*/

        s.source.UnPause();
    }

    public void UnPauseAll()
    {
        musicIsPaused = false;
        foreach (Sound s in sounds)
        {
            s.source.Pause();
            s.source.UnPause();
        }
    }

    public void PlayButtonSFX()
    {
        Play("SFXButton", 1f);
    }
}
