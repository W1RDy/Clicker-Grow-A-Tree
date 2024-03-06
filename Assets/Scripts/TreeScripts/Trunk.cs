using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Trunk : MonoBehaviour, IGrowable
{
    [SerializeField] private Transform spawnPoint;
    private TrunkPart _trunkPartPrefab;
    private float _maxHeight = 0;
    private TrunkPart _heighestTrunkPart;
    private Action<Transform> _relativeObjCallback;
    private CoroutineQueue _coroutineQueue;
    private IGrowable _topFilledGrowable;
    private Action<IGrowable> _endCoroutineCallback;

    private SaveService _saveService;
    private Action SaveData;
    private List<TrunkPart> _trunksQueue = new List<TrunkPart>();
    private Action<Transform> DestroyOddTrunkCallback;
    private IndexDistributor _indexDistributor;

    private Action Unsubscribe;

    public void InitializeTrunk(TrunkPart trunkPartPrefab, Action<Transform> relativeObjCallback, CoroutineQueue coroutineQueue)
    {
        _coroutineQueue = coroutineQueue;
        _trunkPartPrefab = trunkPartPrefab;
        _endCoroutineCallback = growable => _topFilledGrowable = growable;
        _indexDistributor = new IndexDistributor();
        SetRelativeCallback(relativeObjCallback);

        _saveService = ServiceLocator.Instance.Get<SaveService>();

        SaveData = () =>
        {
            _saveService.SaveTrunk(_trunksQueue);
        };

        DestroyOddTrunkCallback = (transform) =>
        {
            var trunkPart = transform.GetComponent<TrunkPart>();
            trunkPart.Destroying -= DestroyOddTrunkCallback;
            _trunksQueue.Remove(trunkPart);
            _indexDistributor.AddFreeIndex(trunkPart.Index);
        };

        Unsubscribe = () =>
        {
            _saveService.SaveDataOnQuit -= SaveData;
            _saveService.QuitApplication -= Unsubscribe;
        };

        _saveService.QuitApplication += Unsubscribe;

        _saveService.SaveDataOnQuit += SaveData;

        foreach (var config in _saveService.DataContainer.TrunkSaveConfig.OrderBy(config => config.PositionY))
        {
            SpawnTrunkPart(transform.InverseTransformPoint(new Vector2(0, config.PositionY)), config.FillValue, config.Index);
        }
    }

    public void SetRelativeCallback(Action<Transform> relativeObjCallback)
    {
        _relativeObjCallback = relativeObjCallback;
        Debug.Log(_relativeObjCallback);
    }

    public void Grow(float sumHeight)
    {
        if (sumHeight >= _maxHeight)
        {
            _heighestTrunkPart.Grow(1);
            Debug.Log("Hey");
            AddNewTrunkPart();
        }
        else
        {
            _heighestTrunkPart.Grow((sumHeight / _heighestTrunkPart.Height) % 1);
        }
    }

    private void SpawnTrunkPart(Vector2 pos, float fillValue, int index)
    {
        _heighestTrunkPart = Instantiate(_trunkPartPrefab, Vector2.zero, Quaternion.identity);
        if (_topFilledGrowable == null) _topFilledGrowable = _heighestTrunkPart;
        _heighestTrunkPart.transform.SetParent(transform);
        _heighestTrunkPart.transform.localPosition = pos;
        _heighestTrunkPart.Index = index;

        _heighestTrunkPart.InitializeTrunk(_relativeObjCallback, _coroutineQueue, fillValue, _endCoroutineCallback);

        _trunksQueue.Add(_heighestTrunkPart);
        _heighestTrunkPart.Destroying += DestroyOddTrunkCallback;

        _maxHeight = pos.y + _heighestTrunkPart.Height;
    }

    private void AddNewTrunkPart()
    {
        var spawnHeight = _maxHeight;
        SpawnTrunkPart(new Vector2(0, spawnHeight), 0, _indexDistributor.GetFreeIndex());
    }

    public Vector2 GetFilledTopLocalPoint()
    {
        return _topFilledGrowable.GetFilledTopLocalPoint();
    }

    public Vector2 GetFilledTopGlobalPoint()
    {
        if (_topFilledGrowable as TrunkPart == null) return GetMaxTopPoint(); 
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

    public int GetIndex()
    {
        return 0;
    }

    public Vector3 GetMaxTopPoint()
    {
        return _heighestTrunkPart.GetFilledTopGlobalPoint();
    }
}