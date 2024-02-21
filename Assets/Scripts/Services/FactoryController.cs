using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FactoryController
{
    private IFactory _factory;
    private GameObject _container;

    public FactoryController(IFactory factory, GameObject container)
    {
        _factory = factory;
        _factory.LoadResources();
        _container = container;
    }

    public MonoBehaviour[] SpawnByFactory(SpawnBranchSettings[] spawnSettings, Transform relativeObj)
    {
        var spawnedObjs = new Branch[spawnSettings.Length];
        for (int i = 0; i < spawnSettings.Length; i++)
        {
            var rotation = new Quaternion
            {
                eulerAngles = spawnSettings[i].IsSpawnToRight ? new Vector3(0, 0, -spawnSettings[i].SpawnRot) : new Vector3(0, 0, spawnSettings[i].SpawnRot)
            };
            var position = relativeObj.TransformPoint(spawnSettings[i].SpawnPos);

            spawnedObjs[i] = _factory.Create(position, rotation, _container.transform) as Branch;
        }
        return spawnedObjs;
    }

    public MonoBehaviour[] SpawnByFactoryWithRandomSettings(SpawnBranchSettings minSpawnBranchSettings, SpawnBranchSettings maxSpawnBranchSettings, int branchCounts, Transform relativeObj)
    {
        var spawnSettings = new SpawnBranchSettings[branchCounts];
        for (int i = 0; i < branchCounts; i++)
        {
            var isSpawnToRight = i % 2 == 0;

            var spawnPosX = isSpawnToRight ? maxSpawnBranchSettings.SpawnPos.x : minSpawnBranchSettings.SpawnPos.x;
            var spawnPosY = Random.Range(minSpawnBranchSettings.SpawnPos.y, maxSpawnBranchSettings.SpawnPos.y);
            var spawnRot = Random.Range(minSpawnBranchSettings.SpawnRot, maxSpawnBranchSettings.SpawnRot);

            spawnSettings[i] = new SpawnBranchSettings(new Vector2(spawnPosX, spawnPosY), spawnRot, isSpawnToRight);
        }

        return SpawnByFactory(spawnSettings, relativeObj);
    }
}

[Serializable]
public class SpawnBranchSettings
{
    [SerializeField] private Vector2 _spawnPos;
    [SerializeField] private float _spawnRot;
    [SerializeField] private bool _isSpawnToRight;

    public Vector2 SpawnPos => _spawnPos;
    public float SpawnRot => _spawnRot;
    public bool IsSpawnToRight => _isSpawnToRight;

    public SpawnBranchSettings(Vector2 spawnPos, float spawnRot, bool isSpawnToRight)
    {
        _spawnPos = spawnPos;
        _spawnRot = spawnRot;
        _isSpawnToRight = isSpawnToRight;
    }
}
