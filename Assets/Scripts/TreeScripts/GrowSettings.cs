using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GrowSettings", menuName = "GrowSettings/new GrowSettings")]
public class GrowSettings : ScriptableObject
{
    [SerializeField, Range(0, 1)] private float _trunkGrowSpeed = 0.02f;
    [SerializeField, Range(0, 1)] private float _branchesGrowSpeed = 0.1f;
    [SerializeField, Min(0)] private int _branchesCount = 2;
    [SerializeField, Min(0)] private int _branchingValue = 1;

    public float TrunkGrowSpeed
    {
        get => _trunkGrowSpeed;
        set
        {
            if ( _trunkGrowSpeed < value && value < 1)
            {
                _trunkGrowSpeed = value;
            }
        }
    }
    public float BranchesGrowSpeed
    {
        get => _branchesGrowSpeed;
        set
        {
            if (_branchesGrowSpeed < value && value < 1)
            {
                _branchesGrowSpeed = value;
            }
        }
    }
    public int BranchesCount
    {
        get => _branchesCount;
        set
        {
            if ( _branchesCount < value)
            {
                _branchesCount = value;
            }
        }
    }
    public int BranchingValue
    {
        get => _branchingValue;
        set
        {
            if ( _branchingValue < value)
            {
                _branchingValue = value;
            }
        }
    }
}
