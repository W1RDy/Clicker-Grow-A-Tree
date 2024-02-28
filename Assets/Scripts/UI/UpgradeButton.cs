using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private UpgradeType _upgradeType;
    [SerializeField] private float value;
    private ButtonService _buttonService;
    private Text _buttonText;

    private void Start()
    {
        _buttonService = ServiceLocator.Instance.Get<ButtonService>();
        _buttonText = GetComponentInChildren<Text>();
        SetValueInText();
    }

    private void SetValueInText()
    {
        _buttonText.text += " + " + value;
    }

    public void Upgrade()
    {
        switch (_upgradeType)
        {
            case UpgradeType.TrunkSpeed:
                _buttonService.UpgradeTrunkSpeed(value);
                break;
            case UpgradeType.BranchSpeed:
                _buttonService.UpgradeBranchSpeed(value);
                break;
            case UpgradeType.BranchingValue:
                _buttonService.UpgradeBranchingValue((int)Mathf.Floor(value));
                break;
            case UpgradeType.BranchCount:
                _buttonService.UpgradeBranchCounts((int)Mathf.Floor(value));
                break;
        }
    }
}

public enum UpgradeType
{
    TrunkSpeed,
    BranchSpeed,
    BranchingValue,
    BranchCount
}
