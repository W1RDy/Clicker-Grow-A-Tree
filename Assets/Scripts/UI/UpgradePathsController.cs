using System;
using UnityEngine;

public class UpgradePathsController : MonoBehaviour, IService
{
    [SerializeField] private UpgradePath[] _upgradePathsMobile;
    [SerializeField] private UpgradePath[] _upgradePathsPC;
    [SerializeField] private UpgradeConfig[] _upgradeConfigs;
    private GrowSettings _growSettings;
    private bool _allPathsActivated;

    public void InitializeUpgradePaths(GrowSettings growSettings, CoinsSpawnSettings coinsSpawnSettings)
    {
        _growSettings = growSettings;
        UpgradePath[] upgradePaths = Screen.height < Screen.width ? _upgradePathsPC : _upgradePathsMobile;

        foreach (var upgradePath in upgradePaths)
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
        foreach (var upgradePath in _upgradePathsMobile)
        {
            if (upgradePath.UpgradeType == upgradeType)
            {
                upgradePath.ActivateUpgradePath();
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
