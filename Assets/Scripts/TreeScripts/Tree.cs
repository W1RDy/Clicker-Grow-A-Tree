using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour, IService, IGrowable
{
    [SerializeField] private Trunk _trunk;
    [SerializeField] private TrunkPart _trunkPartPrefab;
    private CoroutineQueue _coroutineQueue;

    private float _height = 0;
    private float _trunkHeight;
    public GrowSettings GrowSettings {get; private set;}

    public void SetGrowSettings(GrowSettings growSettings)
    {
        GrowSettings = growSettings;
    }

    public void InitializeTree(Action<Transform> relativeObjCallback)
    {
        _coroutineQueue = new CoroutineQueue(this, 12);
        _trunk.InitializeTrunk(_trunkPartPrefab, relativeObjCallback, _coroutineQueue);
        _trunkHeight = _trunkPartPrefab.transform.GetChild(0).localScale.y * _trunkPartPrefab.GetComponentInChildren<SpriteRenderer>().sprite.bounds.size.y;
    }

    public void Grow(float growValue)
    {
        if (!_coroutineQueue.IsCrowded)
        {
            _height += _trunkHeight * growValue;
            _trunk.Grow(_height);
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
