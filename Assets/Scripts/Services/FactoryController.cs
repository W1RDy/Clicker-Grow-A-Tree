using System;
using UnityEngine;

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

    public MonoBehaviour[] SpawnByFactory(SpawnBranchSettings[] spawnSettings)
    {
        var spawnedObjs = new Branch[spawnSettings.Length];
        for (int i = 0; i < spawnSettings.Length; i++)
        {
            var rotation = new Quaternion
            {
                eulerAngles = new Vector3(0, 0, spawnSettings[i].spawnRot)
            };
            spawnedObjs[i] = _factory.Create(spawnSettings[i].spawnPos, rotation, _container.transform) as Branch;
        }
        return spawnedObjs;
    }

    public MonoBehaviour[] SpawnByFactoryWithRandomSettings(SpawnBranchSettings[] spawnBranchSettings)
    {

    }
}

[Serializable]
public class SpawnBranchSettings
{
    public Vector2 spawnPos;
    public float spawnRot;
    public bool isSpawnToRight;
}
