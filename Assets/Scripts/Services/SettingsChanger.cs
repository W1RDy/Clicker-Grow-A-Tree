using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsChanger : IService
{
    private GrowSettings _settings;

    public SettingsChanger(GrowSettings settings)
    {
        _settings = settings;
    }

    public void ChangeGrowTrunkSpeed(float value)
    {
        _settings.TrunkGrowSpeed += value;
    }

    public void ChangeGrowBranchSpeed(float value)
    {
        _settings.BranchesGrowSpeed += value;
    }

    public void ChangeBranchCount(int value)
    {
        _settings.BranchesCount += value;
    }

    public void ChangeBranchingValue(int value)
    {
        _settings.BranchingValue += value;
    }
}
