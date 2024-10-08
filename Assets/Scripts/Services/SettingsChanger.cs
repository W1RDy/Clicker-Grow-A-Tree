using System;
using UnityEngine;

public class SettingsChanger : IService
{
    private GrowSettings _growSettings;
    private CoinsSpawnSettings _coinsSpawnSettings;
    private SaveService _saveService;

    private Action SaveData;
    private Action Unsubscribe;

    public SettingsChanger(GrowSettings settings, CoinsSpawnSettings coinsSpawnSettings)
    {
        _growSettings = settings;
        _coinsSpawnSettings = coinsSpawnSettings;
        _saveService = ServiceLocator.Instance.Get<SaveService>();

        SaveData = () =>
        {
            _saveService.SaveGrowSettings(_growSettings);
            _saveService.SaveCoinsSettings(_coinsSpawnSettings);
        };

        Unsubscribe = () =>
        {
            _saveService.SaveDataOnQuit -= SaveData;
            _saveService.QuitApplication -= Unsubscribe;
        };

        _saveService.QuitApplication += Unsubscribe;
        _saveService.SaveDataOnQuit += SaveData;
    }

    public void ChangeGrowTrunkSpeed(float value)
    {
        _growSettings.TrunkGrowSpeed += value;
        ChangeCoinsSpawnSettings();
    }

    public void ChangeGrowBranchSpeed(float value)
    {
        _growSettings.BranchesGrowSpeed += value;
        ChangeCoinsSpawnSettings();
    }

    public void ChangeBranchCount(int value)
    {
        _growSettings.BranchesCount += value;
        ChangeCoinsSpawnSettings();
    }

    public void ChangeBranchingValue(int value)
    {
        _growSettings.BranchingValue += value;
        ChangeCoinsSpawnSettings();
    }

    public void ChangeCoinsCosts(int value)
    {
        _coinsSpawnSettings.CoinsCosts += value;
        _saveService.SaveAllData();
    }

    private void ChangeCoinsSpawnSettings()
    {
        for (int i = 0; i < _growSettings.BranchingValue + 1; i++)
        {
            _coinsSpawnSettings.SpawnChances[i].spawnChance = (int)Mathf.Round((1f / (_growSettings.BranchingValue + 1)) * 100);
        }

        var branchesCount = _growSettings.BranchesCount * _growSettings.BranchingValue;
        var suggestCoinsCount = (int)Mathf.Floor(Mathf.Lerp(1, _coinsSpawnSettings.MaxCoins, _growSettings.UpgradeProgress));
        _coinsSpawnSettings.CoinsCount = Mathf.Clamp(suggestCoinsCount, 1, branchesCount + 1);
        _saveService.SaveAllData();
    }
}
