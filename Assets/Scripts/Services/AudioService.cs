using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioService : IService
{
    private Dictionary<string, AudioConfig> _audios = new Dictionary<string, AudioConfig>();
    public AudioConfig LastAudioConfig { get; set; }

    public AudioService(AudioData audioData)
    {
        foreach (var config in audioData.AudioConfigs)
        {
            _audios.Add(config.Index, config);
        }
    }

    public AudioConfig GetAudio(string index)
    {
        return _audios[index];
    }
}

[Serializable]
public class AudioConfig
{
    [SerializeField] private string _index;
    [SerializeField] private AudioClip _clip;
    [SerializeField] private float _volume;

    public float Volume => _volume;
    public AudioClip Clip => _clip;
    public string Index => _index;
}
