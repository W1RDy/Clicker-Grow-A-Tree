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

    public SpawnBranchSettings MinBranchSetting => _minBranchSetting;
    public SpawnBranchSettings MaxBranchSetting => _maxBranchSetting;
    public float DistanceBetweenBranch => _distanceBetweenBranch;
    public int BranchCount => _branchCount;
}
