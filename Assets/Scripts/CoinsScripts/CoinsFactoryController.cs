using System;
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

    public void SpawnCoins(Transform relativeObj)
    {
        for (var i = 0; i < _coinsSpawnSettings.CoinsCount; i++)
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
            position = GetRandomPosition(growable);
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
        var randomOffset = Random.Range(0, growable.GetMaxHeight() - growable.GetFilledTopLocalPoint().y - 0.3f);
        return new Vector2(0, growable.GetFilledTopLocalPoint().y + randomOffset);
    }

    public IGrowable GetRandomGrowable(Transform relativeObj)
    {
        var growbaleLevel = GetRandomGrowableLevel();
        if (growbaleLevel > 0)
        {
            var branches = _growablesService.GetBranches(growbaleLevel, relativeObj);
            Debug.Log(branches.Count);
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
        Debug.Log(random);
        Debug.Log(_coinsSpawnSettings.SpawnChances[1].spawnChance);
        foreach (var chance in _coinsSpawnSettings.SpawnChances)
        {
            Debug.Log(chance.spawnChance);
            sum += chance.spawnChance;
            Debug.Log(sum);
            if (sum >= random)
            {
                growableLevel = chance.level;
                break;
            }
        }
        Debug.Log(growableLevel);
        return growableLevel;
    }
}

[Serializable]
public class SpawnChances
{
    public int level;
    public int spawnChance;
}
