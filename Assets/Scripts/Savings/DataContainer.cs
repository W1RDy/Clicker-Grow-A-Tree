﻿using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataContainer", menuName = "Data/new DataContainer")]
public class DataContainer : ScriptableObject
{
    [SerializeField] private bool _isDefaultData = false;
    [SerializeField] private List<BranchSaveConfig> _branchConfigs = new List<BranchSaveConfig>();
    [SerializeField] private List<TrunkSaveConfig> _trunkSaveConfig = new List<TrunkSaveConfig>();
    //[SerializeField] private List<CoinsSaveConfig> _coinsConfigs = new List<CoinsSaveConfig>();
    [SerializeField] private GrowSaveConfig _growConfig;
    [SerializeField] private CoinsSaveSettingsConfig _coinsSpawnConfig;
    [SerializeField] private float _height;
    [SerializeField] private int _score;
    [SerializeField] private int _coins;
    [SerializeField] private Vector3 _cameraPos;

    public List<BranchSaveConfig> BranchConfigs { get => _branchConfigs; set => _branchConfigs = value; }
    public List<TrunkSaveConfig> TrunkSaveConfig { get => _trunkSaveConfig; set => _trunkSaveConfig = value; }
    //public List<CoinsSaveConfig> CoinsSaveConfigs { get => _coinsConfigs; set => _coinsConfigs = value; }
    public GrowSaveConfig GrowConfig { get =>_growConfig; set => _growConfig = value; }
    public CoinsSaveSettingsConfig CoinsSpawnConfig { get => _coinsSpawnConfig; set => _coinsSpawnConfig = value; }
    public bool IsDefaultData { get => _isDefaultData; set => _isDefaultData = value; }
    public float Height { get => _height; set => _height = value; }
    public int Score { get => _score; set => _score = value; }
    public int Coins { get => _coins; set => _coins = value; }
    public Vector3 CameraPos { get => _cameraPos; set => _cameraPos = value; }

    public void SetDefaultSettings(GrowSettings growSettings, CoinsSpawnSettings coinsSpawnSettings)
    {
        IsDefaultData = true;
        _trunkSaveConfig = new List<TrunkSaveConfig>(1) { new TrunkSaveConfig(new Vector2(0, -3), 0, 0) };
        _growConfig = new GrowSaveConfig(growSettings);
        _coinsSpawnConfig = new CoinsSaveSettingsConfig(coinsSpawnSettings);
        _height = 0;
        _score = 0;
        _coins = 0;
        _cameraPos = new Vector3(0, 0, -10);
    }
}

[Serializable]
public class BranchSaveConfig
{
    [SerializeField] private int _index;
    [SerializeField] private int _branchLevel;
    [SerializeField] private bool _isRight;

    [SerializeField] private int _relativeIndex;

    [SerializeField] private Vector3 _position;
    [SerializeField] private float _rotation;
    [SerializeField] private float _fillValue;

    public int Index => _index;
    public Vector3 Position => _position;
    public float Rotation => _rotation;
    public float FillValue { get => _fillValue; set => _fillValue = value; }
    public bool IsRight => _isRight;
    public int BranchLevel => _branchLevel;
    public int RelativeIndex => _relativeIndex;

    public BranchSaveConfig(int index, Vector3 position, float rotation, bool isRight, int branchLevel, int relativeIndex)
    {
        _index = index;
        _position = position;
        _rotation = rotation;
        _isRight = isRight;
        _branchLevel = branchLevel;
        _relativeIndex = relativeIndex;
    }
}

[Serializable]
public class TrunkSaveConfig
{
    [SerializeField] private Vector3 _position;
    [SerializeField] private float _fillValue;
    [SerializeField] private int _index;
    public Vector3 Position => _position;
    public float FillValue { get => _fillValue; set => _fillValue = value; }
    public int Index => _index;

    public TrunkSaveConfig(Vector3 position, float fillValue, int index)
    {
        _position = position;
        FillValue = fillValue;
        _index = index;
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
