using UnityEngine;

[CreateAssetMenu(fileName = "CoinsSpawnSettings", menuName = "SpawnSettings/new CoinsSpawnSettings")]
public class CoinsSpawnSettings : ScriptableObject
{
    [SerializeField] private SpawnChances[] _spawnChances;
    [SerializeField] private int _coinsCount;
    [SerializeField] private int _maxCoins;
    public SpawnChances[] SpawnChances => _spawnChances;
    public int MaxCoins => _maxCoins;
    public int CoinsCount
    {
        get => _coinsCount;
        set
        {
            if (value > _coinsCount && value < _maxCoins)
            {
                _coinsCount = value;
            }
        }
    }
}
