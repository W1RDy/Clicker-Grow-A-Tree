using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADVService : MonoBehaviour, IService
{
    [SerializeField] private float _timeBetweenADV;
    [SerializeField] private float _warningTimeBeforeADV = 3f;
    private ADVWarning _ADVWarning;
    private float _remainingTime;
    private float _startTime;
    private bool _isStopShowing = true;
    private AudioPlayer _audioPlayer;

    private void Start()
    {
        _ADVWarning = GetComponent<ADVWarning>();
        _audioPlayer = ServiceLocator.Instance.Get<AudioPlayer>();
        StartADVShowing();
    }

    public void ActivateADVForReward(float rewardValue)
    {
        _audioPlayer.SetSettings(false);
        InteractorWithBrowser.GetRewardForAdv(rewardValue);
    }

    public void ActivateADV()
    {
        _remainingTime = 0;
        _audioPlayer.SetSettings(false);
        InteractorWithBrowser.ShowAdversity();
    }

    public void StopADVShowing()
    {
        if (!_isStopShowing)
        {
            _isStopShowing = true;
            _remainingTime -= (Time.time - _startTime);
        }
    }

    public void StartADVShowing()
    {
        if (_isStopShowing)
        {
            _isStopShowing = false;
            StartCoroutine(WaitingCoroutine());
        }
    }

    public IEnumerator WaitingCoroutine()
    {
        while (true)
        {
            _startTime = Time.time;
            if (_remainingTime <= 0) _remainingTime = _timeBetweenADV - _warningTimeBeforeADV;

            if (_isStopShowing)
            {
                break;
            }
            yield return new WaitForSeconds(_remainingTime);
            if (_isStopShowing)
            {
                break;
            }

            _ADVWarning.ActivateWarning();
            yield return new WaitForSeconds(_warningTimeBeforeADV);
            ActivateADV();
            _ADVWarning.DeactivateWarning();
        }
    }
}
