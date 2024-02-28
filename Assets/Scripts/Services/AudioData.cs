using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "Data/new AudioData")]
public class AudioData : ScriptableObject
{
    [SerializeField] private AudioConfig[] _audioConfigs;
    public AudioConfig[] AudioConfigs => _audioConfigs;
}
