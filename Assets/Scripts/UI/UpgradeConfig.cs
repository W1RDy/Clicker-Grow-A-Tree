using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeSetting", menuName = "Settings/new UpgradeSetting")]
public class UpgradeConfig : ScriptableObject
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
