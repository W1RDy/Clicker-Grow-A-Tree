using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePathsController : MonoBehaviour, IService
{
    [SerializeField] private UpgradePath[] _upgradePathsMobile;
    [SerializeField] private UpgradePath[] _upgradePathsPC;
    [SerializeField] private UpgradeConfig[] _upgradeConfigs;
    private UpgradePath[] _upgradePaths; 
    private GrowSettings _growSettings;
    private CoinsSpawnSettings _coinsSpawnSettings;
    private bool _allPathsActivated;

    private SaveService _saveService;
    private Action SaveData;
    private Action Unsubscribe;

    public void InitializeUpgradePaths(GrowSettings growSettings, CoinsSpawnSettings coinsSpawnSettings)
    {
        _growSettings = growSettings;
        _coinsSpawnSettings = coinsSpawnSettings;
        _saveService = ServiceLocator.Instance.Get<SaveService>();
        _upgradePaths = Screen.height < Screen.width ? _upgradePathsPC : _upgradePathsMobile;

        foreach (var upgradePath in _upgradePaths)
        {
            var upgradeConfig = FindUpgradeConfig(upgradePath.UpgradeType);

            UpgradePathSaveConfig upgradeSaveConfig;
            if (!_saveService.DataContainer.IsDefaultData) upgradeSaveConfig = FindUpgradeSaveConfig(upgradePath.UpgradeType);
            else upgradeSaveConfig = new UpgradePathSaveConfig(upgradePath.UpgradeType, upgradeConfig.UpgradeValues[0], upgradeConfig.UpgradeCosts[0], 0);

            upgradePath.InitializeUpgradePath(growSettings, coinsSpawnSettings, upgradeConfig, upgradeSaveConfig);
        }

        SaveData = () =>
        {
            var _saveConfigs = new List<UpgradePathSaveConfig>();
            foreach (var upgradePath in _upgradePaths)
            {
                _saveConfigs.Add(upgradePath.GetSaveConfig());
            }

            _saveService.SaveUpgradePaths(_saveConfigs);
        };

        Unsubscribe = () =>
        {
            _saveService.SaveDataOnQuit -= SaveData;
            _saveService.QuitApplication -= Unsubscribe;
        };

        _saveService.QuitApplication += Unsubscribe;

        _saveService.SaveDataOnQuit += SaveData;
    }

    private UpgradeConfig FindUpgradeConfig(UpgradeType upgradeType)
    {
        foreach (var upgradeConfig in _upgradeConfigs)
        {
            if(upgradeConfig.UpgradeType == upgradeType) return Instantiate(upgradeConfig);
        }
        throw new ArgumentNullException("UpgradeConfig with type " + upgradeType + " doesn't exist!");
    }

    private UpgradePathSaveConfig FindUpgradeSaveConfig(UpgradeType upgradeType)
    {
        foreach (var upgradeConfig in _saveService.DataContainer.UpgradePathSaveConfigs)
        {
            if (upgradeConfig.UpgradeType == upgradeType) return upgradeConfig;
        }
        throw new ArgumentNullException("UpgradeConfig with type " + upgradeType + " doesn't exist!");
    }

    public void ActivateUpgradePath(UpgradeType upgradeType)
    {
        ActivateDeactivatePath(upgradeType, true);
    }

    public void DeactivateUpgradePath(UpgradeType upgradeType)
    {
        ActivateDeactivatePath(upgradeType, false);
    }

    private void ActivateDeactivatePath(UpgradeType upgradeType, bool isActivate)
    {
        foreach (var upgradePath in _upgradePaths)
        {
            if (upgradePath.UpgradeType == upgradeType)
            {
                if (isActivate) upgradePath.ActivateUpgradePath();
                else upgradePath.DeactivateUpgradePath();
                break;
            }
        }
    }

    private void Update()
    {
        if (ServiceLocator.Instance.IsRegistered)
        {
            if (!_allPathsActivated)
            {
                if (_growSettings.BranchingValue != 0)
                {
                    ActivateUpgradePath(UpgradeType.BranchCount);
                    if (_growSettings.BranchesCount != 0)
                    {
                        ActivateUpgradePath(UpgradeType.BranchSpeed);
                        _allPathsActivated = true;
                    }
                }
            }

            if (_growSettings.TrunkGrowSpeed >= _growSettings.MaxTrunkGrowSpeed) DeactivateUpgradePath(UpgradeType.TrunkSpeed);
            if (_growSettings.BranchesGrowSpeed >= _growSettings.MaxBranchGrowSpeed) DeactivateUpgradePath(UpgradeType.BranchSpeed);
            if (_growSettings.BranchingValue >= _growSettings.MaxBranchingValue) DeactivateUpgradePath(UpgradeType.BranchingValue);
            if (_growSettings.BranchesCount >= _growSettings.MaxBranchesCount) DeactivateUpgradePath(UpgradeType.BranchCount);
            if (_coinsSpawnSettings.CoinsCosts >= _coinsSpawnSettings.MaxCosts) DeactivateUpgradePath(UpgradeType.CoinsCosts);
        }
    }
}
