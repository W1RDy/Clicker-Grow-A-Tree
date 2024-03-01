using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GrowSettings", menuName = "GrowSettings/new GrowSettings")]
public class GrowSettings : ScriptableObject
{
    [SerializeField, Range(0, 0.7f)] private float _trunkGrowSpeed = 0.02f;
    [SerializeField, Range(0, 0.7f)] private float _branchesGrowSpeed = 0.1f;
    [SerializeField, Min(0)] private int _branchesCount = 2;
    [SerializeField, Min(0)] private int _branchingValue = 1;

    private float _maxTrunkGrowSpeed = 0.7f;
    private float _maxBranchesGrowSpeed = 0.7f;
    [SerializeField, Range(1,6)] private int _maxBranchesCount = 6;
    [SerializeField, Range(1,2)] private int _maxBranchingValue = 2;

    private float _upgradeProgress;

    public float MaxTrunkGrowSpeed => _maxTrunkGrowSpeed;
    public float MaxBranchGrowSpeed => _maxBranchesGrowSpeed;
    public int MaxBranchesCount => _maxBranchesCount;
    public int MaxBranchingValue => _maxBranchingValue;

    public float TrunkGrowSpeed
    {
        get => _trunkGrowSpeed;
        set
        {
            if ( _trunkGrowSpeed < value && value <= _maxTrunkGrowSpeed)
            {
                _trunkGrowSpeed = (float)Math.Round(value, 2, MidpointRounding.AwayFromZero);
                UpgradeProgress = value;
            }
            else if (value > _maxTrunkGrowSpeed && _trunkGrowSpeed != _maxTrunkGrowSpeed)
            {
                _trunkGrowSpeed = _maxTrunkGrowSpeed;
                UpgradeProgress = value;
            }
        }
    }
    public float BranchesGrowSpeed
    {
        get => _branchesGrowSpeed;
        set
        {
            if (_branchesGrowSpeed < value && value <= _maxBranchesGrowSpeed)
            {
                _branchesGrowSpeed = (float)Math.Round(value, 2, MidpointRounding.AwayFromZero);
                UpgradeProgress = value;
            }
            else if (value > _branchesGrowSpeed && _branchesGrowSpeed != _maxBranchesGrowSpeed)
            {
                _branchesGrowSpeed = _maxBranchesGrowSpeed;
                UpgradeProgress = value;
            }
        }
    }
    public int BranchesCount
    {
        get => _branchesCount;
        set
        {
            if ( _branchesCount < value && value <= _maxBranchesCount)
            {
                _branchesCount = value;
                UpgradeProgress = value;
            }
            else if (value > _maxBranchesCount && _branchesCount != _maxBranchesCount)
            {
                _branchesCount = _maxBranchesCount;
                UpgradeProgress = value;
            }
        }
    }
    public int BranchingValue
    {
        get => _branchingValue;
        set
        {
            if ( _branchingValue < value && value <= _maxBranchingValue)
            {
                _branchingValue = value;
                UpgradeProgress = value;
            }
            else if (value > _maxBranchingValue && _branchingValue != _maxBranchingValue)
            {
                _branchingValue = _maxBranchingValue;
                UpgradeProgress = value;
            }
        }
    }

    public float UpgradeProgress
    {
        get => _upgradeProgress;
        set
        {
            var _maxUpgradeValue = _maxTrunkGrowSpeed + _maxBranchesGrowSpeed + _maxBranchingValue + _maxBranchesCount;
            var _currentUpgradeValue = _trunkGrowSpeed + _branchingValue + _branchesGrowSpeed + _branchesCount;
            _upgradeProgress = _currentUpgradeValue / _maxUpgradeValue;
        }
    }
}
