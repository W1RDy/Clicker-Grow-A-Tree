using UnityEngine;

[CreateAssetMenu(fileName = "CoinsSpawnSettings", menuName = "SpawnSettings/new CoinsSpawnSettings")]
public class CoinsSpawnSettings : ScriptableObject
{
    [SerializeField] private SpawnChances[] _spawnChances;
    public SpawnChances[] SpawnChances => _spawnChances;
}
