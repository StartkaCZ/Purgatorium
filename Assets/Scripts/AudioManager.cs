using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The singleton Audio Manager.
/// </summary>
public class AudioManager : MonoBehaviour 
{
    /// <summary>
    /// The different music available.
    /// </summary>
    public enum Music
    {
        Menu,
        Game
    }


    /// <summary>
    /// The different sound effects available.
    /// </summary>
    public enum SoundEffect
    {
        
    }



    static AudioManager                         _instance;


    Dictionary<Music, AudioClip[]>              _music;
    Dictionary<SoundEffect, AudioClip>          _soundEffects;

    List<AudioSource>                           _spellEffectSources;
    List<AudioSource>                           _soundEffectSources;
    AudioSource                                 _loopingSoundEffectSource;
    AudioSource                                 _musicSource;

    float                                       _musicVolume;
    float                                       _soundEffectVolume;

    int                                         _musicIndex;

    bool                                        _muteMusic;
    bool                                        _muteSoundEffects;

    const int                                   AUDIO_EFFECT_SOURCES = 20;
    const int                                   AUDIO_SPELL_SOURCES = 18;



    /// <summary>
    /// The singleton of Audio Manager.
    /// </summary>
    /// <returns></returns>
    static public AudioManager Instance()
    {
        if (_instance == null)
        {
            //Create audio manager
            GameObject audioManager = new GameObject("AudioManager");

            //add AudioManager to the object
            _instance = audioManager.AddComponent<AudioManager>();
            //initialize that object
            _instance.Initialise();

            //stop it from destroying itself
            DontDestroyOnLoad(audioManager);
        }

        return _instance;
    }


    /// <summary>
    /// Initialise the Audio Manager and load all of the audio.
    /// </summary>
    private void Initialise()
    {
        // enable the audio
        _muteMusic = false;
        _muteSoundEffects = false;

        _musicIndex = 0;

        // initialise the resources
        _music = new Dictionary<Music, AudioClip[]>(System.Enum.GetValues(typeof(Music)).Length);
        _soundEffects = new Dictionary<SoundEffect, AudioClip>(System.Enum.GetValues(typeof(SoundEffect)).Length);

        // create the audio sources
        // and load the audio content
        LoadAudioSources();
        LoadContent();
        PrepareMusicVolume();
        PrepareEffectsVolume();
    }


    /// <summary>
    /// Create the neccessary audio sources.
    /// </summary>
    private void LoadAudioSources()
    {
        _soundEffectSources = new List<AudioSource>();
        _spellEffectSources = new List<AudioSource>();

        AudioSource audioSource;
        for (int i = 0; i < AUDIO_EFFECT_SOURCES; i++)
        {//adds audio source componenets to the object
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            _soundEffectSources.Add(audioSource);
        }

        for (int i = 0; i < AUDIO_SPELL_SOURCES; i++)
        {//adds audio source componenets to the object
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            _spellEffectSources.Add(audioSource);
        }

        //sets a music source
        audioSource = gameObject.AddComponent<AudioSource>();
        _musicSource = audioSource;
        _musicSource.playOnAwake = true;

        //sets a looping sound effect source
        audioSource = gameObject.AddComponent<AudioSource>();
        _loopingSoundEffectSource = audioSource;
        _loopingSoundEffectSource.playOnAwake = false;
        _loopingSoundEffectSource.loop = false;
    }


    /// <summary>
    /// Load all of the audio content
    /// </summary>
    private void LoadContent()
    {
        // music
        _music.Add(Music.Game, GetAudioContent("Audio/Music/Casual Theme", 4));

        // sound effects


        // UI and Misc sound effects

    }


    private AudioClip[] GetAudioContent(string path)
    {
        AudioClip[] clips = new AudioClip[1]
        {
            Resources.Load<AudioClip>(path)
        };

        return clips;
    }

    private AudioClip[] GetAudioContent(string path, int files=1)
    {
        AudioClip[] clips = new AudioClip[files];

        for (int i = 0; i < files; i++)
        {
            int j = i + 1;
            clips[i] = Resources.Load<AudioClip>(path + " " + j);
        }

        return clips;
    }
    

    private void PrepareMusicVolume()
    {
        string playerPref = EnumHolder.PlayerPref.MusicVolume.ToString();
        float volume = 0.5f;

        if (PlayerPrefs.HasKey(playerPref))
            volume = PlayerPrefs.GetFloat(playerPref);

        SetMusicVolume(volume);
    }


    private void PrepareEffectsVolume()
    {
        string playerPref = EnumHolder.PlayerPref.SoundEffectVolume.ToString();
        float volume = 0.75f;

        if (PlayerPrefs.HasKey(playerPref))
            volume = PlayerPrefs.GetFloat(playerPref);

        SetEffectsVolume(volume);
    }



    void LateUpdate()
    {
        if (_musicSource.isPlaying == false)
        {
            PlayMusic((Music)_musicIndex);

            _musicIndex++;
            if (_musicIndex >= System.Enum.GetValues(typeof(Music)).Length)
                _musicIndex = 0;
        }
    }



    /// <summary>
    /// Play specific music (song)
    /// </summary>
    /// <param name="music"></param>
    public void PlayMusic(Music music)
    {
        if (!_muteMusic)
        {// if music isn't muted
            AudioClip clip = PickRandomMusicClip(music);

            if (_musicSource.clip != clip)
            {// if the music clip isn't already playing
                _musicSource.Stop();
                _musicSource.clip = clip;
                _musicSource.Play();
            }
        }
    }

    private AudioClip PickRandomMusicClip(Music music)
    {
        AudioClip[] clips = _music[music];
        AudioClip clip = clips[0];

        if (clips.Length > 1)
        {
            int index = Random.Range(0, clips.Length);
            clip = clips[index];
        }

        return clip;
    }



    /// <summary>
    /// Play a specific sound effect.
    /// </summary>
    /// <param name="soundEffect"></param>
    public void PlaySoundEffect(SoundEffect soundEffect, float volume = 1.0f)
    {
        if (!_muteSoundEffects)
        {// if not muted
            AudioClip clip = _soundEffects[soundEffect];
            bool played = false;

            for (int i = 0; i < _soundEffectSources.Count; i++)
            {//look for an avaialable channel
                if (!_soundEffectSources[i].isPlaying)
                {//if its free play the sound effect
                    _soundEffectSources[i].PlayOneShot(clip, _soundEffectVolume * volume);
                    played = true;
                    break;
                }
            }

            if (!played)
            {
                Debug.LogWarning("Not played Sound Effect: " +
                                    soundEffect.ToString() +
                                 " no audio source available");
                GenerateExtraSoundEffectSource(clip, volume);
            }
        }
    }


    private void GenerateExtraSoundEffectSource(AudioClip clip, float volume)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.PlayOneShot(clip, _soundEffectVolume * volume);
        _soundEffectSources.Add(audioSource);
    }



    /// <summary>
    /// mutes all of the auio
    /// </summary>
    /// <param name="mute"></param>
    public void Mute(bool mute)
    {
        _muteMusic = mute;
        _muteSoundEffects = mute;
        AudioListener.pause = mute;
    }


    public void SetMusicVolume(float value)
    {
        string playerPref = EnumHolder.PlayerPref.MusicVolume.ToString();
        PlayerPrefs.SetFloat(playerPref, value);

        _musicVolume = value;
        _musicSource.volume = _musicVolume;

        if (_musicVolume == 0)
            _muteMusic = true;
        else if (_muteMusic)
            _muteMusic = false;
    }


    public void SetEffectsVolume(float value)
    {
        string playerPref = EnumHolder.PlayerPref.SoundEffectVolume.ToString();
        PlayerPrefs.SetFloat(playerPref, value);

        _soundEffectVolume = value;

        if (_soundEffectVolume == 0)
            _muteSoundEffects = true;
        else if (_muteSoundEffects)
            _muteSoundEffects = false;
    }



    public float MusicVolume
    {
        get { return _musicVolume; }
    }

    public float SoundEffectVolume
    {
        get { return _soundEffectVolume; }
    }
}
