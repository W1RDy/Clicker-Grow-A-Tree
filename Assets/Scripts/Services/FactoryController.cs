using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FactoryController
{
    private IFactory _factory;
    private GameObject _container;
    private List<Vector2> _busyPositions = new List<Vector2>();

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

    public MonoBehaviour[] SpawnByFactoryWithRandomSettings(BranchSpawnSettingsConfig spawnSettingsConfig, Transform relativeObj)
    {
        var spawnSettings = RandomizeBranchPoints(spawnSettingsConfig, relativeObj);

        return SpawnByFactory(spawnSettings, relativeObj);
    }

    private SpawnBranchSettings[] RandomizeBranchPoints(BranchSpawnSettingsConfig spawnSettingsConfig, Transform relativeObj)
    {
        var spawnSettings = new SpawnBranchSettings[spawnSettingsConfig.BranchCount];
        for (int i = 0; i < spawnSettingsConfig.BranchCount; i++)
        {
            spawnSettings[i] = RandomizeSpawnSetting(spawnSettingsConfig, i % 2 == 0, relativeObj);
        }
        return spawnSettings;
    }

    private SpawnBranchSettings RandomizeSpawnSetting(BranchSpawnSettingsConfig spawnSettingsConfig, bool isSpawnToRight, Transform relativeObj)
    {
        float spawnRot;
        Vector2 spawnPos;
        int tryCount = 0;

        var maxSpawnBranchSettings = spawnSettingsConfig.MaxBranchSetting;
        var minSpawnBranchSettings = spawnSettingsConfig.MinBranchSetting;

        while (true)
        {
            tryCount++;
            var spawnPosX = isSpawnToRight ? maxSpawnBranchSettings.SpawnPos.x : minSpawnBranchSettings.SpawnPos.x;
            var spawnPosY = Random.Range(minSpawnBranchSettings.SpawnPos.y, maxSpawnBranchSettings.SpawnPos.y);
            spawnRot = Random.Range(minSpawnBranchSettings.SpawnRot, maxSpawnBranchSettings.SpawnRot);

            if (tryCount < 50)
            {
                spawnPos = new Vector2(spawnPosX, spawnPosY);
                if (CheckPosition(spawnPos, relativeObj, spawnSettingsConfig.DistanceBetweenBranch)) break;
            }
            else
            {
                Debug.Log("All pos is busy!");
                spawnPos = new Vector2(spawnPosX, maxSpawnBranchSettings.SpawnPos.y + spawnSettingsConfig.DistanceBetweenBranch);
                break;
            }
        }

        _busyPositions.Add(relativeObj.TransformPoint(spawnPos));

        return new SpawnBranchSettings(spawnPos, spawnRot, isSpawnToRight);
    }


    private bool CheckPosition(Vector2 spawnPos, Transform relativeObj, float distanceBetweenBranch)
    {
        var position = relativeObj.TransformPoint(spawnPos);
        foreach (var busyPos in _busyPositions)
        {
            if (Mathf.Abs(busyPos.x - position.x) < 0.001f && Mathf.Abs(busyPos.y - position.y) < distanceBetweenBranch) return false;
        }
        return true;
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
