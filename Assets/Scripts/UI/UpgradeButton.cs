using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UpgradeButton : MonoBehaviour
{
    private UpgradeType _upgradeType;
    private float _value;
    private int _cost;
    private ButtonService _buttonService;
    private Text _costText;
    private Action UpgradedCallback;
    [SerializeField] private Button _button;
    public Button Button => _button;

    private void Start()
    {
        StartCoroutine(WaitWhileLoaded());
    }

    private IEnumerator WaitWhileLoaded()
    {
        yield return new WaitUntil(() => ServiceLocator.Instance.IsRegistered);
        _buttonService = ServiceLocator.Instance.Get<ButtonService>();
    }

    public void InitializeButton(UpgradeType upgradeType, float upgradeValue, int upgradeCost, Action callback)
    {
        _upgradeType = upgradeType;
        _costText = GetComponentInChildren<Text>();
        SetUpgradeParameters(upgradeValue, upgradeCost);

        UpgradedCallback = callback;
    }

    public void SetUpgradeParameters(float upgradeValue, int upgradeCost)
    {
        _value = upgradeValue;
        _cost = upgradeCost;
        SetCostText();
    }

    private void SetCostText()
    {
        if (_cost == 0)
        {
            _costText.text = "";
        }
        else _costText.text = _cost.ToString();
    }

    public void Upgrade()
    {
        switch (_upgradeType)
        {
            case UpgradeType.TrunkSpeed:
                _buttonService.UpgradeTrunkSpeed(_value, _cost, UpgradedCallback);
                break;
            case UpgradeType.BranchSpeed:
                _buttonService.UpgradeBranchSpeed(_value, _cost, UpgradedCallback);
                break;
            case UpgradeType.BranchingValue:
                _buttonService.UpgradeBranchingValue((int)Mathf.Floor(_value), _cost, UpgradedCallback);
                break;
            case UpgradeType.BranchCount:
                _buttonService.UpgradeBranchCounts((int)Mathf.Floor(_value), _cost, UpgradedCallback);
                break;
            case UpgradeType.CoinsCosts:
                _buttonService.UpgradeCoinsCostsForADV((int)_value, UpgradedCallback);
                break;
        }
    }
}

public enum UpgradeType
{
    TrunkSpeed,
    BranchSpeed,
    BranchingValue,
    BranchCount,
    CoinsCosts
}
