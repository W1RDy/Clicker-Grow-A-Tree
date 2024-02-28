using UnityEngine;

public class AudioInitializer : MonoBehaviour
{
    [SerializeField] private string _musicIndex;

    private void Start()
    {
        var audioPlayer = ServiceLocator.Instance.Get<AudioPlayer>();
        if (_musicIndex != "") audioPlayer.PlayMusic(_musicIndex);
    }
}
