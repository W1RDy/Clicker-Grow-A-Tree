using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BranchSpawnSettings", menuName = "SpawnSettings/new BranchSettings")]
public class BranchSpawnSettingsConfig : ScriptableObject
{
    [SerializeField] private SpawnBranchSettings _minBranchSetting;
    [SerializeField] private SpawnBranchSettings _maxBranchSetting;
    [SerializeField] private float _distanceBetweenBranch = 1.2f;
    [SerializeField] private int _branchCount = 2;
    [SerializeField] private int _maxBranchCount;

    public SpawnBranchSettings MinBranchSetting => _minBranchSetting;
    public SpawnBranchSettings MaxBranchSetting => _maxBranchSetting;
    public float DistanceBetweenBranch
    {
        get => _distanceBetweenBranch;
        set
        {
            if (value.GetType() == typeof(float) && value > 0.2f)
                _distanceBetweenBranch = value;
        }
    }

    public int BranchCount
    {
        get => _branchCount; 
        set
        {
            if (value > _branchCount && value <= _maxBranchCount) _branchCount = value;
        } 
    }
}

[Serializable]
public class SpawnBranchSettings
{
    [SerializeField] private Vector2 _spawnPos;
    [SerializeField] private float _spawnRot;
    [SerializeField] private bool _isSpawnToRight;

    public Vector2 SpawnPos => _spawnPos;
    public float SpawnRot => _spawnRot;
    public bool IsSpawnToRight => _isSpawnToRight;

    public SpawnBranchSettings(Vector2 spawnPos, float spawnRot, bool isSpawnToRight)
    {
        _spawnPos = spawnPos;
        _spawnRot = spawnRot;
        _isSpawnToRight = isSpawnToRight;
    }
}
