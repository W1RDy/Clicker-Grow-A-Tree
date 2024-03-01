using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrowserRequestHandler : MonoBehaviour
{
    private AudioPlayer _audioPlayer;
    private ButtonService _buttonService;

    private void Start()
    {
        _audioPlayer = ServiceLocator.Instance.Get<AudioPlayer>();
        _buttonService = ServiceLocator.Instance.Get<ButtonService>();
    }

    public void SetAudioSettings(bool isAudio)
    {
        Debug.Log("audio");
        _audioPlayer.SetSettings(isAudio);
    }

    public void GetReward(float value)
    {
        Debug.Log("reward");
        _buttonService.UpgradeCoinsCosts((int)value);
    }
}
