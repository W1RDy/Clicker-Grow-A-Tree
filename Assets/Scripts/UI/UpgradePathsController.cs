using System;
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

    public void InitializeUpgradePaths(GrowSettings growSettings, CoinsSpawnSettings coinsSpawnSettings)
    {
        _growSettings = growSettings;
        _coinsSpawnSettings = coinsSpawnSettings;
        _upgradePaths = Screen.height < Screen.width ? _upgradePathsPC : _upgradePathsMobile;

        foreach (var upgradePath in _upgradePaths)
        {
            var upgradeConfig = FindUpgradeConfig(upgradePath.UpgradeType);
            upgradePath.InitializeUpgradePath(growSettings, coinsSpawnSettings, upgradeConfig);
        }
    }

    private UpgradeConfig FindUpgradeConfig(UpgradeType upgradeType)
    {
        foreach (var upgradeConfig in _upgradeConfigs)
        {
            if(upgradeConfig.UpgradeType == upgradeType) return upgradeConfig;
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

[Serializable]
public class UpgradeConfig
{
    [SerializeField] private UpgradeType _upgradePath;
    [SerializeField] private float[] _startUpgradeValue;
    [SerializeField] private int[] _startUpgradeCost;

    [SerializeField] private float _upgradeValueChanges;
    [SerializeField] private float _upgradeCostChanges;
    [SerializeField] private float _changeValueIntensity;
    [SerializeField] private float _changeCostIntensity;

    public UpgradeType UpgradeType => _upgradePath;
    public float UpgradeValueChanges => _upgradeValueChanges;
    public float UpgradeCostChanges => _upgradeCostChanges;
    public float[] UpgradeValues => _startUpgradeValue;
    public int[] UpgradeCosts => _startUpgradeCost;
    public float ChangeValueIntensity => _changeValueIntensity;
    public float ChangeCostIntensity => _changeCostIntensity;
}
