using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour, IService
{
    private AudioService _audioService;
    private AudioSource _audioSource;
    private bool _isAudio = true;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _audioService = ServiceLocator.Instance.Get<AudioService>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void SetSettings(bool isMusic)
    {
        _isAudio = isMusic;
        if (isMusic) PlayMusic(_audioService.LastAudioConfig.Index);
        else StopMusic();
    }

    public void PlayMusic(string index)
    {
        AudioConfig audio = _audioService.GetAudio(index);
        if ((_audioService.LastAudioConfig == null || _audioService.LastAudioConfig.Index != index || !_audioSource.isPlaying) && _isAudio)
        {
            _audioSource.Stop();
            _audioSource.volume = audio.Volume;
            _audioSource.clip = audio.Clip;
            _audioSource.Play();
        }
        _audioService.LastAudioConfig = audio;
    }

    public void StopMusic()
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
    }

    public void ContinueMusic()
    {
        if (_isAudio && !_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }

    public void PlaySounds(string index)
    {
        if (_isAudio)
        {
            var audio = _audioService.GetAudio(index);
            _audioSource.PlayOneShot(audio.Clip, audio.Volume);
        }
    }
}
