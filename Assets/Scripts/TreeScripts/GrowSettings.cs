using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GrowSettings", menuName = "GrowSettings/new GrowSettings")]
public class GrowSettings : ScriptableObject
{
    [SerializeField, Range(0, 1)] private float _trunkGrowSpeed = 0.02f;
    [SerializeField, Range(0, 1)] private float _branchesGrowSpeed = 0.1f;
    [SerializeField] private int _branchesCount = 2;
    [SerializeField, Min(1)] private int _branchingValue = 1;

    public float TrunkGrowSpeed => _trunkGrowSpeed;
    public float BranchesGrowSpeed => _branchesGrowSpeed;
    public int BranchesCount => _branchesCount;
    public int BranchingValue => _branchingValue;
}
