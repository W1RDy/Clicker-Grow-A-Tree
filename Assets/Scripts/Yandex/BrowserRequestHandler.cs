using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrowserRequestHandler : MonoBehaviour
{
    private AudioPlayer _audioPlayer;
    private ButtonService _buttonService;
    private ADVService _advService;

    private void Start()
    {
        _audioPlayer = ServiceLocator.Instance.Get<AudioPlayer>();
        _buttonService = ServiceLocator.Instance.Get<ButtonService>();
        _advService = ServiceLocator.Instance.Get<ADVService>();
    }

    public void ContinueMusic()
    {
        _audioPlayer.ContinueMusic();
    }

    public void GetReward(float value)
    {
        _buttonService.UpgradeCoinsCosts((int)value);
    }

    public void StartShowingADV()
    {
        _advService.StartADVShowing();
    }
}
