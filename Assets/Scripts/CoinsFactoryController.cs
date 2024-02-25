using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CoinsFactoryController : MonoBehaviour
{
    [SerializeField] private SpawnChances[] _spawnChances;
    private GrowablesService _growablesService;
    private CoinsFactory _coinsFactory;

    private void Start()
    {
        _growablesService = ServiceLocator.Instance.Get<GrowablesService>();
        var container = new GameObject("Coins").transform;
        _coinsFactory = new CoinsFactory(container);
        _coinsFactory.LoadResources();
        SpawnCoin();
    }

    public void SpawnCoin()
    {
        var growable = GetRandomGrowable();
        var randomOffset = Random.Range(0, growable.GetMaxHeight());

        var position = new Vector2(0, growable.GetFilledTopLocalPoint().y + randomOffset / growable.GetRelativeGrowable().GetMaxHeight());
        position = growable.GetGrowableTransform().TransformPoint(position);

        var coin = _coinsFactory.Create(position, Quaternion.identity, null) as Coin;
        coin.ConnectGrowable(growable);
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

    public int GetRandomGrowableLevel()
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
