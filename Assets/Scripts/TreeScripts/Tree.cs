using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, IService, IGrowable
{
    [SerializeField] private Trunk _trunk;
    [SerializeField] private TrunkPart _trunkPartPrefab;
    private Branch[] branches;
    private float _height = 0;
    private Action<Transform[]> _branchMarkersCallback;
    private FactoryController _branchFactoryController;

    private void Awake()
    {
        var branchContainer = new GameObject(name + "Branches");
        branchContainer.transform.SetParent(transform);
        _branchFactoryController = new FactoryController(new BranchFactory(), branchContainer);
        _branchMarkersCallback = markers => SpawnBranches(markers);
        Debug.Log(_branchMarkersCallback);
        _trunk.InitializeTrunk(_trunkPartPrefab, _branchMarkersCallback);
    }

    public void Grow(float growValue)
    {
        _height += _trunkPartPrefab.transform.localScale.y * growValue;
        _trunk.Grow(_height);
        GrowBranches(growValue);
    }

    private void SpawnBranches(Transform[] markers)
    {
        branches = _branchFactoryController.SpawnByFactory(markers) as Branch[];
    }

    private void GrowBranches(float growValue)
    {
        foreach (Branch branch in branches)
        {
            if (GetTopPoint().y > branch.transform.position.y)
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
