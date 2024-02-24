using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, IService, IGrowable
{
    [SerializeField] BranchSpawnSettingsConfig[] _spawnSettingsConfig;
    [SerializeField] private Trunk _trunk;
    [SerializeField] private TrunkPart _trunkPartPrefab;
    [SerializeField, Min(1)] private int _branchingValue = 1;

    private float _height = 0;
    private List<Branch> _branches = new List<Branch>();
    private BranchSpawnSettingsConfig[] _spawnSettings;

    private FactoryController[] _branchFactoryControllers;
    private Action<Transform> _trunkPartCallback;

    private void Awake()
    {
        InitializeSpawnSettings();
        InitializeBranchFactoryControllers();

        _trunkPartCallback = relativeObj => SpawnBranches(relativeObj, 1);
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

        _branchFactoryControllers = new FactoryController[_spawnSettings.Length];
        for (int i = 0; i < _branchFactoryControllers.Length; i++)
        {
            var factory = new BranchFactory(i + 1);
            factory.LoadResources();
            _branchFactoryControllers[i] = new FactoryController(factory, branchContainer);
        }
    }

    public void Grow(float growValue)
    {
        _height += _trunkPartPrefab.transform.localScale.y * growValue;
        _trunk.Grow(_height);
        GrowBranches(growValue);
    }

    private void SpawnBranches(Transform relativeObj, int branchLevel)
    {
        if (branchLevel > _branchingValue) return;

        var newBranches = _branchFactoryControllers[branchLevel - 1].SpawnByFactoryWithRandomSettings(_spawnSettings[branchLevel - 1], relativeObj) as Branch[];
        var nextBranchLevel = branchLevel + 1;
        foreach (var branch in newBranches)
        {
            branch.InitializeBranch(relativeObj);
            _branches.Add(branch);
            SpawnBranches(branch.transform, nextBranchLevel);
        }
    }

    private void GrowBranches(float growValue)
    {
        var branches = new List<Branch>(_branches);
        foreach (Branch branch in branches)
        {
            if (branch == null) _branches.Remove(branch);
            else branch.Grow(growValue * branch.Height);
        }
    }

    public float GetHeight()
    {
        return _height;
    }

    public Vector2 GetFilledTopLocalPoint()
    {
        return _trunk.GetFilledTopLocalPoint();
    }

    public Vector2 GetTopTreePoint()
    {
        var topPoint = _trunk.transform.TransformPoint(new Vector2(0, -_trunkPartPrefab.transform.localScale.y / 2 + _height));
        return topPoint;
    }
}
