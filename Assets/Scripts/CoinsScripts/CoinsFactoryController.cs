﻿using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CoinsFactoryController
{
    private CoinsSpawnSettings _coinsSpawnSettings;
    private GrowablesService _growablesService;
    private CoinsFactory _coinsFactory;
    private BusyPosService _busyPosService;

    public CoinsFactoryController(CoinsSpawnSettings coinsSpawnSettings)
    {
        _coinsSpawnSettings = coinsSpawnSettings;
        _growablesService = ServiceLocator.Instance.Get<GrowablesService>();
        var container = new GameObject("Coins").transform;
        _coinsFactory = new CoinsFactory(container);
        _coinsFactory.LoadResources();
        _busyPosService = new BusyPosService();
    }

    public void SpawnCoins(int count, Transform relativeObj)
    {
        for (var i = 0; i < count; i++)
        {
            SpawnCoin(relativeObj);
        }
    }

    private void SpawnCoin(Transform relativeObj)
    {
        IGrowable growable;
        Vector2 position;
        while (true)
        {
            growable = GetRandomGrowable(relativeObj);
            Debug.Log(growable.GetGrowableTransform().name);
            position = GetRandomPosition(growable);
            Debug.Log(position);
            if (!_busyPosService.CheckPosition(position, growable.GetGrowableTransform(), 2f))
            {
                Debug.Log("All Busy");
                continue;
            }

            _busyPosService.AddBusyPos(growable.GetGrowableTransform(), position);
            position = growable.GetGrowableTransform().TransformPoint(position);
            break;
        }
        var coin = _coinsFactory.Create(position, Quaternion.identity, null) as Coin;
        coin.ConnectGrowable(growable);
    }

    public Vector2 GetRandomPosition(IGrowable growable)
    {
        bool growableIsTree = growable.GetGrowableTransform() == growable.GetRelativeGrowable().GetGrowableTransform();
        var minOffset = growableIsTree ? 0.5f : 0;
        var randomOffset = Random.Range(minOffset, growable.GetMaxHeight() - growable.GetFilledTopLocalPoint().y - 0.3f);
        var growableSizeLocalTransform = growableIsTree ? growable.GetMaxHeight() : 1;
        Debug.Log(growable.GetFilledTopLocalPoint().y);
        return new Vector2(0, growable.GetFilledTopLocalPoint().y + randomOffset / growableSizeLocalTransform);
    }

    public IGrowable GetRandomGrowable(Transform relativeObj)
    {
        var growbaleLevel = GetRandomGrowableLevel();
        if (growbaleLevel > 0)
        {
            var branches = _growablesService.GetBranches(growbaleLevel, relativeObj);
            var randomGrowable = branches[Random.Range(0, branches.Count)];
            return randomGrowable;
        }
        else return _growablesService.GetTree();
    }

    private int GetRandomGrowableLevel()
    {
        int random = Random.Range(1, 101);
        var growableLevel = 0;
        int sum = 0;
        foreach (var chance in _coinsSpawnSettings.SpawnChances)
        {
            sum += chance.spawnChance;
            if (sum >= random)
            {
                growableLevel = chance.level;
                break;
            }
        }
        return growableLevel;
    }
}

[Serializable]
public class SpawnChances
{
    public int level;
    public int spawnChance;
}
