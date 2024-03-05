using System;
using UnityEngine;

public class UpgradePath : MonoBehaviour
{
    [SerializeField] private UpgradeType _upgradeType;
    private UpgradeConfig _upgradeConfig;
    private float _upgradeValue;
    private int _upgradeCost;
    private int _upgradeCount;

    public UpgradeType UpgradeType => _upgradeType;

    private UpgradePathIndicatorsController _indicatorsController;
    private UpgradePathView _pathView;
    [SerializeField] private UpgradeButton _upgradeButton;
    private GrowSettings _growSettings;
    private CoinsSpawnSettings _coinsSpawnSettings;

    [SerializeField] private bool _isActivated;
    private SaveService _saveService;

    public void InitializeUpgradePath(GrowSettings growSettings, CoinsSpawnSettings coinsSpawnSettings, UpgradeConfig upgradeConfig, UpgradePathSaveConfig upgradePathSaveConfig)
    {
        _growSettings = growSettings;
        _coinsSpawnSettings = coinsSpawnSettings;
        _upgradeConfig = upgradeConfig;
        _saveService = ServiceLocator.Instance.Get<SaveService>();

        _upgradeValue = upgradePathSaveConfig.UpgradeValue;
        _upgradeCost = upgradePathSaveConfig.UpgradeCost;
        _upgradeCount = upgradePathSaveConfig.UpgradeCount;

        _indicatorsController = GetComponent<UpgradePathIndicatorsController>();
        _pathView = GetComponent<UpgradePathView>();

        _upgradeButton.InitializeButton(_upgradeType, _upgradeValue, _upgradeCost, ChangeUpgradesParameters);

        var parameters = GetParameters(_upgradeType);
        _indicatorsController.InitializeIndicators(_upgradeValue, parameters);

        _pathView.Initialize(_upgradeButton.Button);
        if (!_isActivated) _pathView.DeactivatePath();
    }

    private void ChangeUpgradesParameters()
    {
        _upgradeCount ++;
        var parameters = GetParameters(UpgradeType);
        var valueByProgress = 1 / (Mathf.Clamp(1 - parameters.currentParameter / parameters.maxParameter, 0.01f, 1));

        if (_upgradeConfig.UpgradeValues.Length - 1 >= _upgradeCount) _upgradeValue = _upgradeConfig.UpgradeValues[_upgradeCount];
        else _upgradeValue += (float)Math.Round(_upgradeConfig.UpgradeValueChanges * valueByProgress, 2, MidpointRounding.ToEven) * _upgradeConfig.ChangeValueIntensity;

        if (_upgradeConfig.UpgradeCosts.Length - 1 >= _upgradeCount) _upgradeCost = _upgradeConfig.UpgradeCosts[_upgradeCount];
        else _upgradeCost += (int)(Mathf.Floor(_upgradeConfig.UpgradeCostChanges * valueByProgress) * _upgradeConfig.ChangeCostIntensity);

        ChangeParametersView();
    }

    private void ChangeParametersView()
    {
        var parameters = GetParameters(_upgradeType);

        if (parameters.currentParameter == parameters.maxParameter)
        {
            _upgradeValue = 0;
            _upgradeCost = 0;
        }
        else if (parameters.currentParameter + _upgradeValue > parameters.maxParameter)
        {
            _upgradeValue = parameters.maxParameter - parameters.currentParameter;
        }

        _upgradeButton.SetUpgradeParameters(_upgradeValue, _upgradeCost);
        _indicatorsController.UpdateIndicators(_upgradeValue, parameters.currentParameter);

        if (parameters.currentParameter >= parameters.maxParameter) DeactivateUpgradePath();

        _saveService.SaveAllData();
    }

    public void ActivateUpgradePath()
    {
        if (!_isActivated)
        {
            _isActivated = true;
            _pathView.ActivatePath();
        }
    }

    public void DeactivateUpgradePath()
    {
        if (_isActivated)
        {
            _isActivated = false;
            _pathView.DeactivatePath();
        }
    }

    public UpgradePathSaveConfig GetSaveConfig()
    {
        return new UpgradePathSaveConfig(_upgradeType, _upgradeValue, _upgradeCost, _upgradeCount);
    }

    private (float currentParameter, float maxParameter) GetParameters(UpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case UpgradeType.TrunkSpeed:
                return (_growSettings.TrunkGrowSpeed, _growSettings.MaxTrunkGrowSpeed);
            case UpgradeType.BranchSpeed:
                return (_growSettings.BranchesGrowSpeed, _growSettings.MaxBranchGrowSpeed);
            case UpgradeType.BranchingValue:
                return (_growSettings.BranchingValue, _growSettings.MaxBranchingValue);
            case UpgradeType.BranchCount:
                return (_growSettings.BranchesCount, _growSettings.MaxBranchesCount);
            case UpgradeType.CoinsCosts:
                return (_coinsSpawnSettings.CoinsCosts, 2);
        }
        throw new System.ArgumentNullException("Parameters with type " + upgradeType + " doesn't exist!");
    }
}