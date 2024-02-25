using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonService : MonoBehaviour, IService
{
    private SettingsChanger _settigsChanger;
    private WindowActivator _windowActivator;

    private void Start()
    {
        _settigsChanger = ServiceLocator.Instance.Get<SettingsChanger>();
        _windowActivator = ServiceLocator.Instance.Get<WindowActivator>();
    }

    public void UpgradeTrunkSpeed(float value)
    {
        _settigsChanger.ChangeGrowTrunkSpeed(value);
    }

    public void UpgradeBranchSpeed(float value)
    {
        _settigsChanger.ChangeGrowBranchSpeed(value);
    }

    public void UpgradeBranchingValue(int value)
    {
        _settigsChanger.ChangeBranchingValue(value);
    }

    public void UpgradeBranchCounts(int value)
    {
        _settigsChanger.ChangeBranchCount(value);
    }

    public void ActivateUpgradeWindow()
    {
        _windowActivator.ActivateWindow(WindowType.UpgradeWindow);
    }

    public void DeactivateUpgradeWindow()
    {
        _windowActivator.DeactivateWindow(WindowType.UpgradeWindow);
    }
}
