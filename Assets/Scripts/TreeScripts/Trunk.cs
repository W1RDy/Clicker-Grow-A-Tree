using System;
using UnityEngine;

public class Trunk : MonoBehaviour, IGrowable
{
    [SerializeField] private Transform spawnPoint;
    private TrunkPart _trunkPartPrefab;
    private float _maxHeight = 0;
    private TrunkPart _heighestTrunkPart;
    private Action<Transform> _trunkPartCallback;
    private CoroutineQueue _coroutineQueue;
    private IGrowable _topFilledGrowable;
    private Action<IGrowable> _endCoroutineCallback;

    public void InitializeTrunk(TrunkPart trunkPartPrefab, Action<Transform> trunkPartCallback, CoroutineQueue coroutineQueue)
    {
        _coroutineQueue = coroutineQueue;
        _trunkPartCallback = trunkPartCallback;
        _trunkPartPrefab = trunkPartPrefab;
        _endCoroutineCallback = growable => _topFilledGrowable = growable;

        AddNewTrunkPart();
    }

    public void Grow(float sumHeight)
    {
        if (sumHeight >= _maxHeight)
        {
            //Debug.Log(sumHeight);
            //Debug.Log(_maxHeight);
            _heighestTrunkPart.Grow(1);
            AddNewTrunkPart();
        }
        else
        {
            //Debug.Log((sumHeight / _heighestTrunkPart.Height) % 1);
            _heighestTrunkPart.Grow((sumHeight / _heighestTrunkPart.Height) % 1);
        }
    }

    private void AddNewTrunkPart()
    {
        var spawnHeight = _maxHeight == 0 ? 0 : _maxHeight  + _trunkPartPrefab.Height;
        _heighestTrunkPart = Instantiate(_trunkPartPrefab, Vector2.zero, Quaternion.identity);
        if (_topFilledGrowable == null) _topFilledGrowable = _heighestTrunkPart;
        _heighestTrunkPart.transform.SetParent(transform);
        _heighestTrunkPart.transform.localPosition = new Vector2(0, spawnHeight);
        _heighestTrunkPart.InitializeTrunk(_trunkPartCallback, _coroutineQueue, _endCoroutineCallback);

        _maxHeight = _maxHeight + _heighestTrunkPart.Height;
    }

    public Vector2 GetFilledTopLocalPoint()
    {
        return _topFilledGrowable.GetFilledTopLocalPoint();
    }

    public Vector2 GetFilledTopGlobalPoint()
    {
        return _topFilledGrowable.GetFilledTopGlobalPoint();
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