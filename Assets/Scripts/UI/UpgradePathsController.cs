using System;
using UnityEngine;

public class UpgradePathsController : MonoBehaviour, IService
{
    [SerializeField] private UpgradePath[] _upgradePaths;
    [SerializeField] private UpgradeConfig[] _upgradeConfigs;
    private GrowSettings _growSettings;
    private bool _allPathsActivated;

    public void InitializeUpgradePaths(GrowSettings growSettings)
    {
        _growSettings = growSettings;
        foreach (var upgradePath in _upgradePaths)
        {
            var upgradeConfig = FindUpgradeConfig(upgradePath.UpgradeType);
            upgradePath.InitializeUpgradePath(growSettings, upgradeConfig);
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
        foreach (var upgradePath in _upgradePaths)
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
            Debug.Log(_growSettings.BranchingValue);
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
    [SerializeField] private float _startUpgradeValue;
    [SerializeField] private int _startUpgradeCost;

    [SerializeField] private float _upgradeValueChanges;
    [SerializeField] private int _upgradeCostChanges;

    public UpgradeType UpgradeType => _upgradePath;
    public float UpgradeValueChanges => _upgradeValueChanges;
    public int UpgradeCostChanges => _upgradeCostChanges;
    public float StartUpgradeValue => _startUpgradeValue;
    public int StartUpgradeCost => _startUpgradeCost;
}
