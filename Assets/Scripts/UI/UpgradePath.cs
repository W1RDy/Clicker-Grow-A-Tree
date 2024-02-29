using System;
using UnityEngine;

public class UpgradePath : MonoBehaviour
{
    [SerializeField] private UpgradeType _upgradeType;
    private UpgradeConfig _upgradeConfig;
    private float _upgradeValue;
    private int _upgradeCost;

    public UpgradeType UpgradeType => _upgradeType;

    private UpgradePathIndicatorsController _indicatorsController;
    private UpgradePathView _pathView;
    [SerializeField] private UpgradeButton _upgradeButton;
    private GrowSettings _growSettings;

    [SerializeField] private bool _isActivated;

    public void InitializeUpgradePath(GrowSettings growSettings, UpgradeConfig upgradeConfig)
    {
        _growSettings = growSettings;
        _upgradeConfig = upgradeConfig;

        _upgradeValue = upgradeConfig.StartUpgradeValue;
        _upgradeCost = upgradeConfig.StartUpgradeCost;

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
        var valueByProgress = 1 / (Mathf.Clamp(1 - _growSettings.UpgradeProgress, 0.01f, 1));
        _upgradeValue += (float)Math.Round(_upgradeConfig.UpgradeValueChanges * valueByProgress, 2, MidpointRounding.ToEven);
        _upgradeCost += (int)Mathf.Floor(valueByProgress);

        _upgradeButton.SetUpgradeParameters(_upgradeValue, _upgradeCost);
        ChangeParametersView();
    }

    private void ChangeParametersView()
    {
        var parameters = GetParameters(_upgradeType);
        _indicatorsController.UpdateIndicators(_upgradeValue, parameters.currentParameter);
    }

    public void ActivateUpgradePath()
    {
        if (!_isActivated)
        {
            _isActivated = true;
            _pathView.ActivatePath();
        }
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
        }
        throw new System.ArgumentNullException("Parameters with type " + upgradeType + " doesn't exist!");
    }
}
