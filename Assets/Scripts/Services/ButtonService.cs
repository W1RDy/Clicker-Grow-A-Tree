using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonService : MonoBehaviour, IService
{
    private SettingsChanger _settigsChanger;
    private WindowActivator _windowActivator;
    private AudioPlayer _audioPlayer;
    private CoinsCounter _coinsCounter;

    private void Start()
    {
        _settigsChanger = ServiceLocator.Instance.Get<SettingsChanger>();
        _windowActivator = ServiceLocator.Instance.Get<WindowActivator>();
        _audioPlayer = ServiceLocator.Instance.Get<AudioPlayer>();
        _coinsCounter = ServiceLocator.Instance.Get<CoinsCounter>();
    }

    public void UpgradeTrunkSpeed(float value, int cost, Action callback)
    {
        if (_coinsCounter.Coins >= cost)
        {
            PlayClickSound();
            _settigsChanger.ChangeGrowTrunkSpeed(value);
            RemoveCoins(cost);
            callback?.Invoke();
        }
    }

    public void UpgradeBranchSpeed(float value, int cost, Action callback)
    {
        if (_coinsCounter.Coins >= cost)
        {
            PlayClickSound();
            _settigsChanger.ChangeGrowBranchSpeed(value);
            RemoveCoins(cost);
            callback?.Invoke();
        }
    }

    public void UpgradeBranchingValue(int value, int cost, Action callback)
    {
        if (_coinsCounter.Coins >= cost)
        {
            PlayClickSound();
            _settigsChanger.ChangeBranchingValue(value);
            RemoveCoins(cost);
            callback?.Invoke();
        }
    }

    public void UpgradeBranchCounts(int value, int cost, Action callback)
    {
        if (_coinsCounter.Coins >= cost)
        {
            PlayClickSound();
            _settigsChanger.ChangeBranchCount(value);
            RemoveCoins(cost);
            callback?.Invoke();
        }
    }

    public void ActivateUpgradeWindow()
    {
        PlayClickSound();
        _windowActivator.ActivateWindow(WindowType.UpgradeWindow);
    }

    public void DeactivateUpgradeWindow()
    {
        PlayClickSound();
        _windowActivator.DeactivateWindow(WindowType.UpgradeWindow);
    }

    private void PlayClickSound()
    {
        _audioPlayer.PlaySounds("Click");
    }

    private void RemoveCoins(int coins)
    {
        _coinsCounter.RemoveCoins(coins);
    }
}
