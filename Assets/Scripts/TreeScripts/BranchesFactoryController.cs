﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class BranchesFactoryController 
{
    private IFactory _factory;
    private GameObject _container;
    private BranchSpawnSettingsConfig _spawnSettingsConfig;
    private BusyPosService _busyPosService;

    public BranchesFactoryController(IFactory factory, GameObject container, BranchSpawnSettingsConfig spawnSettingsConfig)
    {
        _factory = factory;
        _factory.LoadResources();
        _container = container;
        _spawnSettingsConfig = spawnSettingsConfig;
        _busyPosService = new BusyPosService();
    }

    public MonoBehaviour[] SpawnByFactory(SpawnBranchSettings[] spawnSettings, Transform relativeObj)
    {
        var spawnedObjs = new Branch[spawnSettings.Length];
        for (int i = 0; i < spawnSettings.Length; i++)
        {
            spawnedObjs[i] = SpawnByFactory(spawnSettings[i], relativeObj) as Branch;
        }
        return spawnedObjs;
    }

    public MonoBehaviour SpawnByFactory(SpawnBranchSettings spawnSetting, Transform relativeObj)
    {
        var rotation = Quaternion.Euler(0, 0, spawnSetting.SpawnRot);

        if (spawnSetting.IsSpawnToRight) rotation = Quaternion.Inverse(rotation);
        rotation = relativeObj.rotation * rotation;
        var position = relativeObj.TransformPoint(spawnSetting.SpawnPos);

        return _factory.Create(position, rotation, _container.transform) ;
    }

    public MonoBehaviour[] SpawnByFactoryWithRandomSettings(Transform relativeObj)
    {
        var spawnSettings = RandomizeBranchPoints(relativeObj);

        return SpawnByFactory(spawnSettings, relativeObj);
    }

    private SpawnBranchSettings[] RandomizeBranchPoints(Transform relativeObj)
    {
        Debug.Log(_spawnSettingsConfig.BranchCount);
        var spawnSettings = new SpawnBranchSettings[_spawnSettingsConfig.BranchCount];
        for (int i = 0; i < _spawnSettingsConfig.BranchCount; i++)
        {
            spawnSettings[i] = RandomizeSpawnSetting(i % 2 == 0, relativeObj);
        }
        return spawnSettings;
    }

    private SpawnBranchSettings RandomizeSpawnSetting(bool isSpawnToRight, Transform relativeObj)
    {
        float spawnRot;
        Vector2 spawnPos;
        int tryCount = 0;

        var maxSpawnBranchSettings = _spawnSettingsConfig.MaxBranchSetting;
        var minSpawnBranchSettings = _spawnSettingsConfig.MinBranchSetting;
        while (true)
        {
            tryCount++;
            var spawnPosX = isSpawnToRight ? maxSpawnBranchSettings.SpawnPos.x : minSpawnBranchSettings.SpawnPos.x;
            
            var spawnPosY = Random.Range(minSpawnBranchSettings.SpawnPos.y, maxSpawnBranchSettings.SpawnPos.y);
            spawnRot = Random.Range(minSpawnBranchSettings.SpawnRot, maxSpawnBranchSettings.SpawnRot);

            if (tryCount < 100)
            {
                spawnPos = new Vector2(spawnPosX / relativeObj.localScale.x, spawnPosY / relativeObj.localScale.y);
                if (!_busyPosService.CheckPosition(spawnPos, relativeObj, _spawnSettingsConfig.DistanceBetweenBranch)) continue;
            }
            else
            {
                //spawnPos = new Vector2(spawnPosX / relativeObj.localScale.x, maxSpawnBranchSettings.SpawnPos.y / relativeObj.localScale.y);
                _spawnSettingsConfig.DistanceBetweenBranch = (maxSpawnBranchSettings.SpawnPos.y - minSpawnBranchSettings.SpawnPos.y) / (_spawnSettingsConfig.BranchCount + 1);
                tryCount = 0;
                Debug.Log("Change");
                continue;
            }
            _busyPosService.AddBusyPos(relativeObj, spawnPos);
            break;

        }
        return new SpawnBranchSettings(spawnPos, spawnRot, isSpawnToRight);
    }
}