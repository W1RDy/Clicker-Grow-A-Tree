using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class FactoryController
{
    private IFactory _factory;
    private GameObject _container;
    private Dictionary<Transform, List<Vector2>> _busyPositions = new Dictionary<Transform, List<Vector2>>(); // need to optimise

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
                eulerAngles = new Vector3(0, 0, spawnSettings[i].SpawnRot)
            };
            if (spawnSettings[i].IsSpawnToRight) rotation = Quaternion.Inverse(rotation);
            rotation = relativeObj.rotation * rotation;
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

            if (tryCount < 100)
            {
                spawnPos = new Vector2(spawnPosX / relativeObj.localScale.x, spawnPosY / relativeObj.localScale.y);
                if (!CheckPosition(spawnPos, relativeObj, spawnSettingsConfig.DistanceBetweenBranch)) continue;
            }
            else
            {
                Debug.LogError("All pos is busy!");
                spawnPos = new Vector2(spawnPosX / relativeObj.localScale.x, maxSpawnBranchSettings.SpawnPos.y / relativeObj.localScale.y);
            }
            UpdateBusyPos(relativeObj, spawnPos);
            break;

        }
        return new SpawnBranchSettings(spawnPos, spawnRot, isSpawnToRight);
    }

    private bool CheckPosition(Vector2 spawnPos, Transform relativeObj, float distanceBetweenBranch)
    {
        if (!_busyPositions.ContainsKey(relativeObj)) return true;

        var position = spawnPos;

        foreach (var busyPos in _busyPositions[relativeObj])
        {
            if (Mathf.Abs(busyPos.x - position.x) < 0.00001f / relativeObj.localScale.x && Mathf.Abs(busyPos.y - position.y) < distanceBetweenBranch / relativeObj.localScale.y)
            {
                return false;
            }
        }
        return true;
    }

    public void UpdateBusyPos(Transform relativeObj, Vector2 position)
    {
        //var dictionaryCopy = new Dictionary<Transform, List<Vector2>>();
        //foreach (var transorm in dictionaryCopy.Keys)
        //{
        //    if (transorm == null)
        //    {
        //        Debug.Log("Clear");
        //        _busyPositions.Remove(transorm);
        //    }
        //}

        List<Vector2> positions;
        if (!_busyPositions.TryGetValue(relativeObj, out positions))
        {
            _busyPositions[relativeObj] = positions = new List<Vector2>();
        }
        positions.Add(position);
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
