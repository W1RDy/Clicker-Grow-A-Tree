using System;
using UnityEngine;

public class Trunk : MonoBehaviour, IGrowable
{
    private TrunkPart _trunkPartPrefab;
    private float _maxHeight = 0;
    private TrunkPart _heighestTrunkPart;
    private Action<Transform> _trunkPartCallback;

    public void InitializeTrunk(TrunkPart trunkPartPrefab, Action<Transform> trunkPartCallback)
    {
        _trunkPartCallback = trunkPartCallback;
        _trunkPartPrefab = trunkPartPrefab;
        AddNewTrunkPart();
    }

    public void Grow(float sumHeight)
    {
        if (sumHeight >= _maxHeight)
        {
            _heighestTrunkPart.Grow(1);
            AddNewTrunkPart();
        }
        _heighestTrunkPart.Grow((sumHeight / _heighestTrunkPart.Height) % 1);
    }

    private void AddNewTrunkPart()
    {
        var spawnHeight = _maxHeight == 0 ? 0 : _maxHeight + _trunkPartPrefab.Height / 2;
        _heighestTrunkPart = Instantiate(_trunkPartPrefab, Vector2.zero, Quaternion.identity);
        _heighestTrunkPart.transform.SetParent(transform);
        _heighestTrunkPart.transform.localPosition = new Vector2(0, spawnHeight);
        _heighestTrunkPart.InitializeTrunk(_trunkPartCallback);

        _maxHeight = _heighestTrunkPart.transform.localPosition.y + _heighestTrunkPart.Height;
    }

    public Vector2 GetFilledTopLocalPoint()
    {
        return _heighestTrunkPart.GetFilledTopLocalPoint();
    }

    public Vector2 GetFilledTopGlobalPoint()
    {
        return _heighestTrunkPart.GetFilledTopGlobalPoint();
    }

    public float GetMaxHeight()
    {
        return _heighestTrunkPart.GetMaxHeight();
    }

    public IGrowable GetRelativeGrowable()
    {
        return _heighestTrunkPart.GetRelativeGrowable();
    }

    public Transform GetGrowableTransform()
    {
        return _heighestTrunkPart.GetGrowableTransform();
    }
}
