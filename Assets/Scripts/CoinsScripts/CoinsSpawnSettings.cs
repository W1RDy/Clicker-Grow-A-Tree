using UnityEngine;

[CreateAssetMenu(fileName = "CoinsSpawnSettings", menuName = "SpawnSettings/new CoinsSpawnSettings")]
public class CoinsSpawnSettings : ScriptableObject
{
    [SerializeField] private SpawnChances[] _spawnChances;
    [SerializeField] private int _coinsCount;
    [SerializeField] private int _maxCoins;
    [SerializeField] private float _distanceChangeIntensity;
    [SerializeField] private int _coinsCosts;
    [SerializeField] private int _maxCosts;

    public void SetSettings(CoinsSaveSettingsConfig _saveSettings)
    {
        _spawnChances = _saveSettings.SpawnChances;
        _coinsCosts = _saveSettings.CoinsCosts;
        _distanceChangeIntensity = _saveSettings.DistanceChangeIntensity;
        _coinsCount = _saveSettings.CoinsCount;
    }

    public SpawnChances[] SpawnChances => _spawnChances;
    public int MaxCoins => _maxCoins;
    public int CoinsCount
    {
        get => _coinsCount;
        set
        {
            if (value <= _maxCoins)
            {
                _coinsCount = value;
            }
        }
    }
    public float DistanceChangeIntensity => _distanceChangeIntensity;
    public int CoinsCosts 
    {
        get => _coinsCosts;
        set
        {
            if  (_coinsCosts < value && value <= _maxCosts)
            {
                _coinsCosts = value;
            }
            else if (value > _maxCosts && _coinsCosts != _maxCosts)
            {
                _coinsCosts = _maxCoins;
            }
        }
    }

    public int MaxCosts => _maxCosts;
}
