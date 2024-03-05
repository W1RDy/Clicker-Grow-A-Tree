using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
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
    private CancellationTokenSource _cancellationTokenSource;

    private void Start()
    {
        StartCoroutine(WaitWhileRegistered());
    }

    private IEnumerator WaitWhileRegistered()
    {
        yield return new WaitUntil(() => ServiceLocator.Instance.IsRegistered);
        {
            _ADVWarning = GetComponent<ADVWarning>();
            _audioPlayer = ServiceLocator.Instance.Get<AudioPlayer>();
            StartADVShowing();
        }
    }

    public void ActivateADVForReward(float rewardValue)
    {
        _audioPlayer.StopMusic();
        InteractorWithBrowser.GetRewardForAdv(rewardValue);
    }

    public void ActivateADV()
    {
        StopADVShowing();
        _remainingTime = 0;
        _audioPlayer.StopMusic();
        InteractorWithBrowser.ShowAdversity();
    }

    public void StopADVShowing()
    {
        if (!_isStopShowing)
        {
            _isStopShowing = true;
            _remainingTime -= (Time.time - _startTime);
            _cancellationTokenSource.Cancel();
        }
    }

    public void StartADVShowing()
    {
        if (_isStopShowing)
        {
            _isStopShowing = false;
            _cancellationTokenSource = new CancellationTokenSource();
            StartCoroutine(WaitingCoroutine(_cancellationTokenSource.Token));
        }
    }

    public IEnumerator WaitingCoroutine(CancellationToken cancellationToken)
    {
        _startTime = Time.time;
        if (_remainingTime <= 0) _remainingTime = _timeBetweenADV - _warningTimeBeforeADV;
        if (_isStopShowing || cancellationToken.IsCancellationRequested)
        {
            yield break;
        }
        yield return new WaitForSeconds(_remainingTime);
        if (_isStopShowing || cancellationToken.IsCancellationRequested)
        {
            yield break;
        }

        _ADVWarning.ActivateWarning();
        yield return new WaitForSeconds(_warningTimeBeforeADV);
        ActivateADV();
        _ADVWarning.DeactivateWarning();
    }
}
