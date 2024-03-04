using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataContainer", menuName = "Data/new DataContainer")]
public class DataContainer : ScriptableObject
{
    [SerializeField] private List<BranchSaveConfig> _branchConfigs = new List<BranchSaveConfig>();
    [SerializeField] private TrunkSaveConfig _trunkConfig;
    [SerializeField] private List<CoinsSaveConfig> _coinsConfigs = new List<CoinsSaveConfig>();
    [SerializeField] private GrowSaveConfig _growConfig;
    [SerializeField] private CoinsSaveSettingsConfig _coinsSpawnConfig;

    public List<BranchSaveConfig> BranchConfigs { get => _branchConfigs; set => _branchConfigs = value; }
    public TrunkSaveConfig TrunkConfig { get => _trunkConfig; set => _trunkConfig = value; }
    public List<CoinsSaveConfig> CoinsSaveConfigs { get => _coinsConfigs; set => _coinsConfigs = value; }
    public GrowSaveConfig GrowConfig { get =>_growConfig; set => _growConfig = value; }
    public CoinsSaveSettingsConfig CoinsSpawnConfig { get => _coinsSpawnConfig; set => _coinsSpawnConfig = value; }

    public void SetDefaultSettings(GrowSettings growSettings, CoinsSpawnSettings coinsSpawnSettings)
    {
        _growConfig = new GrowSaveConfig(growSettings);
        _coinsSpawnConfig = new CoinsSaveSettingsConfig(coinsSpawnSettings);
    }
}

[Serializable]
public class BranchSaveConfig
{
    [SerializeField] private int _index;
    [SerializeField] private Vector3 _position;
    [SerializeField] private Quaternion _rotation;
    [SerializeField] private float _fillValue;

    public int Index => _index;
    public Vector3 Position => _position;
    public Quaternion Rotation => _rotation;
    public float FillValue { get => _fillValue; set => _fillValue = value; }

    public BranchSaveConfig(int index, Vector3 position, Quaternion rotation)
    {
        _index = index;
        _position = position;
        _rotation = rotation;
        FillValue = 0;
    }
}

[Serializable]
public class TrunkSaveConfig
{
    [SerializeField] private Vector3 _position;
    [SerializeField] private float _fillValue;
    public Vector3 Position => _position;
    public float FillValue { get => _fillValue; set => _fillValue = value; }

    public TrunkSaveConfig(Vector3 position, float fillValue)
    {
        _position = position;
        FillValue = fillValue;
    }
}

[Serializable]
public class CoinsSaveConfig
{
    [SerializeField] private Vector3 _position;

    public Vector3 Position => _position;

    public CoinsSaveConfig(Vector3 position)
    {
        _position = position;
    }
}

[Serializable]
public class GrowSaveConfig
{
    [SerializeField, Range(0, 0.7f)] private float _trunkGrowSpeed = 0.02f;
    [SerializeField, Range(0, 0.7f)] private float _branchesGrowSpeed = 0.1f;
    [SerializeField, Min(0)] private int _branchesCount = 2;
    [SerializeField, Min(0)] private int _branchingValue = 1;

    public float TrunkGrowSpeed => _trunkGrowSpeed;
    public float BranchesGrowSpeed => _branchesGrowSpeed;
    public int BranchesCount => _branchesCount;
    public int BranchingValue => _branchingValue;

    public GrowSaveConfig(GrowSettings growSettings)
    {
        _trunkGrowSpeed = growSettings.TrunkGrowSpeed;
        _branchesGrowSpeed = growSettings.BranchesGrowSpeed;
        _branchesCount = growSettings.BranchesCount;
        _branchingValue = growSettings.BranchingValue;
    }
}

[Serializable]
public class CoinsSaveSettingsConfig
{
    [SerializeField] private SpawnChances[] _spawnChances;
    [SerializeField] private int _coinsCount;
    [SerializeField] private float _distanceChangeIntensity;
    [SerializeField] private int _coinsCosts;

    public SpawnChances[] SpawnChances => _spawnChances;
    public int CoinsCount => _coinsCount;
    public float DistanceChangeIntensity => _distanceChangeIntensity;
    public int CoinsCosts => _coinsCosts;

    public CoinsSaveSettingsConfig(CoinsSpawnSettings settings)
    {
        _spawnChances = settings.SpawnChances;
        _coinsCount = settings.CoinsCount;
        _distanceChangeIntensity = settings.DistanceChangeIntensity;
        _coinsCosts = settings.CoinsCosts;
    }
}
