using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    //Different AudioSources for music and sound.
    [SerializeField] AudioSource _musicSource, _soundSource;
    [SerializeField] AudioMixer _gameAudio;


    private float _volume;
    public float Volume { get => _volume; set => _volume = value; }

    
    private bool _isMuted;
    public bool IsMuted { get => _isMuted; set => _isMuted = value; }

    private void Awake()
    {
        //Ensure there is only one AudioManager.
        if (Instance == null) 
            Instance = this;
        else 
            Destroy(gameObject);

        DontDestroyOnLoad(this);

        //Set default volume value.
        _volume = -5;
    }
    public void PlayMusic(string musicClipName)
    {
        //If music is already playing, stop it.
        _musicSource.Stop();

        //Find matching string from the music folder.
        AudioClip musicClip = Resources.Load<AudioClip>("Audio/Music/" + musicClipName);

        if (musicClip == null)
        {
            Debug.Log(musicClipName + " doesn't exist");
        }
        else
        {
            _musicSource.clip = musicClip;
            _musicSource.Play();
        }

    }
    public void PlaySound(string soundClipName, bool randomPitch = false)
    {
        //Find matching string from the sounds folder.
        AudioClip soundClip = Resources.Load<AudioClip>("Audio/Sounds/" + soundClipName);

        if (soundClip == null)
        {
            Debug.Log(soundClip + " doesn't exist");
            return;
        }
        //Randomize sound pitch.
        if (randomPitch)
        {
            float pitch = Random.Range(0.75f, 1.25f);
            _soundSource.pitch = pitch;
        }
        _soundSource.PlayOneShot(soundClip);
        _soundSource.pitch = 1;
    }
    public void SetVolume(float volume)
    {
        //If volume is -35, player can barely hear it, so mute it completely.
        if (volume == -35)
        {
            volume = -80;
        }
            
        _gameAudio.SetFloat("Volume", volume);

        //Store previous volume value if player wants to mute audio.
        _volume = volume;
    }
    public void ToggleMute()
    {
        if (!_isMuted)
        {
            _isMuted = true;
            _gameAudio.SetFloat("Volume", -80);
        }
        else
        {
            _gameAudio.SetFloat("Volume", _volume);
            _isMuted = false;
        }
    }
}
