using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, IService, IGrowable
{
    [SerializeField] private Trunk _trunk;
    [SerializeField] private TrunkPart _trunkPartPrefab;

    private float _height = 0;
    public GrowSettings GrowSettings {get; private set;}

    public void SetGrowSettings(GrowSettings growSettings)
    {
        GrowSettings = growSettings;
    }

    public void InitializeTree(Action<Transform> relativeObjCallback)
    {
        _trunk.InitializeTrunk(_trunkPartPrefab, relativeObjCallback);
    }

    public void Grow(float growValue)
    {
        _height += _trunkPartPrefab.transform.localScale.y * growValue;
        _trunk.Grow(_height);
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
