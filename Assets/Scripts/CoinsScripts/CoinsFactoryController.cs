using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CoinsFactoryController
{
    private CoinsSpawnSettings _coinsSpawnSettings;
    private GrowablesService _growablesService;
    private CoinsFactory _coinsFactory;
    private BusyPosService _busyPosService;
    private GrowSettings _growSettings;
    private float _spawnDistance = 2f;

    private SaveService _saveService;
    private CoinService _coinService;
    private bool _isFirstSpawn;

    //private Action SaveData;

    public CoinsFactoryController(CoinsSpawnSettings coinsSpawnSettings, GrowSettings growSettings)
    {
        _growSettings = growSettings;
        _coinsSpawnSettings = coinsSpawnSettings;
        _growablesService = ServiceLocator.Instance.Get<GrowablesService>();
        _coinService = new CoinService();

        var container = new GameObject("Coins").transform;
        _coinsFactory = new CoinsFactory(container);
        _coinsFactory.LoadResources();
        _busyPosService = new BusyPosService();

        _saveService = ServiceLocator.Instance.Get<SaveService>();
        _isFirstSpawn = true;

        //SaveData = () =>
        //{
        //    _saveService.SaveCoins(_coinService.GetCoins());
        //    _saveService.SaveDataOnQuit -= SaveData;
        //};

        //_saveService.SaveDataOnQuit += SaveData;
    }

    public void SpawnCoins(Transform relativeObj)
    {
        for (var i = 0; i < _coinsSpawnSettings.CoinsCount; i++)
        {
            SpawnCoin(relativeObj);
        }

        if (_saveService.DataContainer.IsDefaultData && _isFirstSpawn)
        {
            _isFirstSpawn = false;
            var growable = GetRandomGrowable(relativeObj);
            SpawnCoin(relativeObj, 0.5f, growable.GetMaxHeight() - growable.GetFilledTopLocalPoint().y - 0.3f);
        }
        else
        {
            _saveService.SaveAllData();
        }    
    }

    private void SpawnCoin(Transform relativeObj)
    {
        SpawnCoin(relativeObj, 0, 0);
    }

    private void SpawnCoin(Transform relativeObj, float minPoint, float maxPoint)
    {
        IGrowable growable;
        Vector2 position;
        while (true)
        {
            growable = GetRandomGrowable(relativeObj);
            position = GetRandomPosition(growable, minPoint, maxPoint);
            if (!_busyPosService.CheckPosition(position, growable.GetGrowableTransform(), _spawnDistance))
            {
                Debug.Log("All Busy");
                continue;
            }

            _busyPosService.AddBusyPos(growable.GetGrowableTransform(), position);
            position = growable.GetGrowableTransform().TransformPoint(position);
            break;
        }
        var coin = _coinsFactory.Create(position, Quaternion.identity, null) as Coin;
        coin.Initialize(growable, _coinsSpawnSettings);

        Action<Transform> DestroyCallback = transform =>
        {
            var coin = transform.GetComponent<Coin>();
            _coinService.RemoveCoin(coin);
        };

        coin.Destroying += DestroyCallback;
        _coinService.AddCoin(coin);
    }

    private Vector2 GetRandomPosition(IGrowable growable, float minPoint, float maxPoint)
    {
        if (minPoint == 0 && maxPoint == 0)
        {
            return GetRandomPosition(growable);
        }
        var randomOffset = Random.Range(minPoint, maxPoint);
        return new Vector2(0, growable.GetFilledTopLocalPoint().y + randomOffset);
    }

    public Vector2 GetRandomPosition(IGrowable growable)
    {
        var valueByProgressForMinPoint = 1 / (Mathf.Clamp(1 - _growSettings.UpgradeProgress, 0.2f, 1));
        var valueByProgressForMaxPoint = 1 / (Mathf.Clamp(_growSettings.UpgradeProgress, 0.1f, 1));

        valueByProgressForMinPoint *= _coinsSpawnSettings.DistanceChangeIntensity;
        valueByProgressForMaxPoint /= _coinsSpawnSettings.DistanceChangeIntensity;

        var minPoint = Mathf.Clamp(0.4f * valueByProgressForMinPoint, 0.2f, 0.8f);
        var maxPoint = Mathf.Clamp((growable.GetMaxHeight() - growable.GetFilledTopLocalPoint().y - 0.3f) / valueByProgressForMaxPoint, minPoint, growable.GetMaxHeight() - growable.GetFilledTopLocalPoint().y - 0.3f);

        return GetRandomPosition(growable, minPoint, maxPoint);
    }

    public IGrowable GetRandomGrowable(Transform relativeObj)
    {
        var growbaleLevel = GetRandomGrowableLevel();
        if (growbaleLevel > 0)
        {
            var branches = _growablesService.GetBranches(growbaleLevel, relativeObj);
            if (branches.Count == 0) return GetRandomGrowable(relativeObj);
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
        //Debug.Log(random);
        //Debug.Log(_coinsSpawnSettings.SpawnChances[1].spawnChance);
        foreach (var chance in _coinsSpawnSettings.SpawnChances)
        {
            //Debug.Log(chance.spawnChance);
            sum += chance.spawnChance;
            //Debug.Log(sum);
            if (sum >= random)
            {
                growableLevel = chance.level;
                break;
            }
        }
        //Debug.Log(growableLevel);
        return growableLevel;
    }
}

[Serializable]
public class SpawnChances
{
    public int level;
    public int spawnChance;

    public SpawnChances(int level, int spawnChance)
    {
        this.level = level;
        this.spawnChance = spawnChance;
    }
}
