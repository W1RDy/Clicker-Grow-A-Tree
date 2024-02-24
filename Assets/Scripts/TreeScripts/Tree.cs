using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, IService, IGrowable
{
    [SerializeField] BranchSpawnSettingsConfig _spawnSettingsConfig;
    [SerializeField] private Trunk _trunk;
    [SerializeField] private TrunkPart _trunkPartPrefab;

    private float _height = 0;
    private List<Branch> _branches = new List<Branch>();
    private BranchSpawnSettingsConfig _spawnSettings;

    private FactoryController _branchFactoryController;
    private Action<Transform> _trunkPartCallback;

    private void Awake()
    {
        _spawnSettings = Instantiate(_spawnSettingsConfig);
        var branchContainer = new GameObject(name + "Branches");
        branchContainer.transform.SetParent(transform);
        _branchFactoryController = new FactoryController(new BranchFactory(), branchContainer);
        _trunkPartCallback = relativeObj => SpawnBranches(relativeObj);
        _trunk.InitializeTrunk(_trunkPartPrefab, _trunkPartCallback);
    }

    public void Grow(float growValue)
    {
        _height += _trunkPartPrefab.transform.localScale.y * growValue;
        _trunk.Grow(_height);
        GrowBranches(growValue);
    }

    private void SpawnBranches(Transform relativeObj)
    {
        var newBranches = _branchFactoryController.SpawnByFactoryWithRandomSettings(_spawnSettings, relativeObj) as Branch[];
        foreach (var branch in newBranches)
        {
            _branches.Add(branch);
        }
    }

    private void GrowBranches(float growValue)
    {
        var branches = new List<Branch>(_branches);
        foreach (Branch branch in branches)
        {
            if (branch == null) _branches.Remove(branch);
            else if (GetTopPoint().y > branch.transform.position.y)
            {
                branch.Grow(growValue * branch.Height);
            }
        }
    }

    public float GetHeight()
    {
        return _height;
    }

    public Vector2 GetTopPoint()
    {
        var topPoint = _trunk.transform.TransformPoint(new Vector2(0, -_trunkPartPrefab.transform.localScale.y / 2 + _height));
        return topPoint;
    }
}
