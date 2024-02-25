using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CoinsFactoryController : MonoBehaviour
{
    [SerializeField] private SpawnChances[] _spawnChances;
    private GrowablesService _growablesService;
    private CoinsFactory _coinsFactory;
    private BusyPosService _busyPosService;

    private void Start()
    {
        _growablesService = ServiceLocator.Instance.Get<GrowablesService>();
        var container = new GameObject("Coins").transform;
        _coinsFactory = new CoinsFactory(container);
        _coinsFactory.LoadResources();
        _busyPosService = new BusyPosService();
        SpawnCoin();
    }

    public void SpawnCoin()
    {
        IGrowable growable;
        Vector2 position;
        while (true)
        {
            growable = GetRandomGrowable();
            position = GetRandomPosition(growable);

            if (!_busyPosService.CheckPosition(position, growable.GetGrowableTransform(), 2f)) continue;

            _busyPosService.AddBusyPos(growable.GetGrowableTransform(), position);
            position = growable.GetGrowableTransform().TransformPoint(position);
            break;
        }
        var coin = _coinsFactory.Create(position, Quaternion.identity, null) as Coin;
        coin.ConnectGrowable(growable);
    }

    public Vector2 GetRandomPosition(IGrowable growable)
    {
        var randomOffset = Random.Range(0, growable.GetMaxHeight());
        return new Vector2(0, growable.GetFilledTopLocalPoint().y + randomOffset / growable.GetRelativeGrowable().GetMaxHeight());
    }

    public IGrowable GetRandomGrowable()
    {
        var growbaleLevel = GetRandomGrowableLevel();
        if (growbaleLevel > 0)
        {
            var branches = _growablesService.GetBranches(growbaleLevel);
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
        foreach (var chance in _spawnChances)
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
