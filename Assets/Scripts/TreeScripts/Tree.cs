using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, IService, IGrowable
{
    [SerializeField] BranchSpawnSettingsConfig[] _spawnSettingsConfig;
    [SerializeField] private Trunk _trunk;
    [SerializeField] private TrunkPart _trunkPartPrefab;

    private float _height = 0;
    private BranchSpawnSettingsConfig[] _spawnSettings;

    private BranchesFactoryController[] _branchFactoryControllers;
    private GrowSettings _growSettings;

    private Action<Transform> _trunkPartCallback;
    public event Action<int, Branch[]> SpawnNewBranches;

    public void InitializeTree(GrowSettings growSettings)
    {
        _growSettings = growSettings;
        InitializeSpawnSettings();
        InitializeBranchFactoryControllers();

        _trunkPartCallback = relativeObj =>
        {
            foreach (var spawnSetting in _spawnSettings)
            {
                spawnSetting.BranchCount = _growSettings.BranchesCount;
            }

            SpawnBranches(relativeObj, 1);
        };
        _trunk.InitializeTrunk(_trunkPartPrefab, _trunkPartCallback);
    }

    private void InitializeSpawnSettings()
    {
        _spawnSettings = new BranchSpawnSettingsConfig[_spawnSettingsConfig.Length];
        for (int i = 0; i < _spawnSettings.Length; i++)
        {
            _spawnSettings[i] = Instantiate(_spawnSettingsConfig[i]);
        }
    }

    private void InitializeBranchFactoryControllers()
    {
        var branchContainer = new GameObject(name + "Branches");
        branchContainer.transform.SetParent(transform);

        _branchFactoryControllers = new BranchesFactoryController[_spawnSettings.Length];
        for (int i = 0; i < _branchFactoryControllers.Length; i++)
        {
            var factory = new BranchFactory(i + 1);
            factory.LoadResources();
            _branchFactoryControllers[i] = new BranchesFactoryController(factory, branchContainer, _spawnSettings[i]);
        }
    }

    public void Grow(float growValue)
    {
        _height += _trunkPartPrefab.transform.localScale.y * growValue;
        _trunk.Grow(_height);
    }

    private void SpawnBranches(Transform relativeObj, int branchLevel)
    {
        if (branchLevel > _growSettings.BranchingValue) return;

        var newBranches = _branchFactoryControllers[branchLevel - 1].SpawnByFactoryWithRandomSettings(relativeObj) as Branch[];
        var nextBranchLevel = branchLevel + 1;
        foreach (var branch in newBranches)
        {
            branch.InitializeBranch(relativeObj, branchLevel);
            SpawnBranches(branch.transform, nextBranchLevel);
        }
        SpawnNewBranches?.Invoke(branchLevel, newBranches);
    }

    public float GetHeight()
    {
        return _height;
    }

    public Vector2 GetFilledTopLocalPoint()
    {
        return _trunk.GetFilledTopLocalPoint();
    }

    public Vector2 GetFilledTopGlobalPoint()
    {
        return _trunk.GetFilledTopGlobalPoint();
    }

    public float GetMaxHeight()
    {
        return _trunk.GetMaxHeight();
    }

    public IGrowable GetRelativeGrowable()
    {
        return _trunk.GetRelativeGrowable();
    }

    public Transform GetGrowableTransform()
    {
        return _trunk.GetGrowableTransform();
    }
}
