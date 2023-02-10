using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private bool _muteBackgroundMusic = false;
    private bool _muteSoundFx = false;
    public static SoundManager instance;

    private AudioSource _audioSource;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Play();
    }

    public void toggleBackgroundMusic()
    {
        _muteBackgroundMusic = !_muteBackgroundMusic;
        if (_muteBackgroundMusic)
        {
            _audioSource.Stop();
        }
        else
        {
            _audioSource.Play();
        }
    }

    public void toggleSoundFx()
    {
        _muteSoundFx = !_muteSoundFx;
        GameEvents.ToggleSoundFXMethod();
    }

    public bool isBackgroundMusicMuted()
    {
        return _muteBackgroundMusic;
    }

    public bool isSoundFxMuted()
    {
        return _muteSoundFx;
    }

    public void silienceBackgroundMusic(bool silence)
    {
        if (_muteBackgroundMusic == false)
        {
            if (silence)
            {
                _audioSource.volume = 0f;
            }
            else
            {
                _audioSource.volume = 1f;
            }
        }
    }

    
}
