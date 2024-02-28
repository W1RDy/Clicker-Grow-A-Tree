using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonService : MonoBehaviour, IService
{
    private SettingsChanger _settigsChanger;
    private WindowActivator _windowActivator;
    private AudioPlayer _audioPlayer;

    private void Start()
    {
        _settigsChanger = ServiceLocator.Instance.Get<SettingsChanger>();
        _windowActivator = ServiceLocator.Instance.Get<WindowActivator>();
        _audioPlayer = ServiceLocator.Instance.Get<AudioPlayer>();
    }

    public void UpgradeTrunkSpeed(float value)
    {
        PlayClickSound();
        _settigsChanger.ChangeGrowTrunkSpeed(value);
    }

    public void UpgradeBranchSpeed(float value)
    {
        PlayClickSound();
        _settigsChanger.ChangeGrowBranchSpeed(value);
    }

    public void UpgradeBranchingValue(int value)
    {
        PlayClickSound();
        _settigsChanger.ChangeBranchingValue(value);
    }

    public void UpgradeBranchCounts(int value)
    {
        PlayClickSound();
        _settigsChanger.ChangeBranchCount(value);
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
}
