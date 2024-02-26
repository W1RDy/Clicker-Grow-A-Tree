using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsChanger : IService
{
    private GrowSettings _gorwSettings;
    private CoinsSpawnSettings _coinsSpawnSettings;

    public SettingsChanger(GrowSettings settings, CoinsSpawnSettings coinsSpawnSettings)
    {
        _gorwSettings = settings;
        _coinsSpawnSettings = coinsSpawnSettings;
    }

    public void ChangeGrowTrunkSpeed(float value)
    {
        _gorwSettings.TrunkGrowSpeed += value;
    }

    public void ChangeGrowBranchSpeed(float value)
    {
        _gorwSettings.BranchesGrowSpeed += value;
    }

    public void ChangeBranchCount(int value)
    {
        _gorwSettings.BranchesCount += value;
    }

    public void ChangeBranchingValue(int value)
    {
        _gorwSettings.BranchingValue += value;
    }
}
