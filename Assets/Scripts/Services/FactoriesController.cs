using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class FactoriesController : IService
{
    private CoinsFactoryController _coinsFactoryController;
    private BranchesFactoryController[] _branchFactoryControllers;

    private BranchSpawnSettingsConfig[] _spawnSettings;
    private Action<Transform> _trunkRandomSpawnCallback;
    private Action<Transform> _trunksSaveSpawnCallback;
    public event Action<int, Branch[]> SpawnNewBranches;

    private Tree _tree;
    private GrowSettings _growSettings;
    private SaveService _saveService;

    public void InitializeController(BranchSpawnSettingsConfig[] spawnSettingsConfig, CoinsSpawnSettings coinsSpawnSettings, GrowSettings growSettings)
    {
        _tree = ServiceLocator.Instance.Get<Tree>();
        _growSettings = growSettings;
        InitializeSpawnSettings(spawnSettingsConfig);
        InitializeBranchFactoryControllers();
        _coinsFactoryController = new CoinsFactoryController(coinsSpawnSettings, _growSettings);

        _saveService = ServiceLocator.Instance.Get<SaveService>();
        var branchConfigs = _saveService.DataContainer.BranchConfigs;

        InitializeCallbacks();

        if (_saveService.DataContainer.IsDefaultData) _tree.InitializeTree(_trunkRandomSpawnCallback);
        else _tree.InitializeTree(_trunksSaveSpawnCallback);
    }

    private void InitializeCallbacks()
    {
        _trunkRandomSpawnCallback = relativeObj =>
        {
            foreach (var spawnSetting in _spawnSettings)
            {
                spawnSetting.BranchCount = _growSettings.BranchesCount;
            }

            SpawnBranches(relativeObj, 1);
            SpawnCoins(relativeObj);
        };

        int saveSpawnCounts = 0;

        _trunksSaveSpawnCallback = relativeObj =>
        {
            saveSpawnCounts++;
            if (saveSpawnCounts > _saveService.DataContainer.TrunkSaveConfig.Count)
            {
                saveSpawnCounts = 0;
                _tree.SetRelativeCallback(_trunkRandomSpawnCallback);
                _trunkRandomSpawnCallback.Invoke(relativeObj);
            }

            foreach (var spawnSetting in _spawnSettings)
            {
                spawnSetting.BranchCount = _growSettings.BranchesCount;
            }

            SpawnBranches(_saveService.DataContainer.BranchConfigs, relativeObj, 1);
        };
    }

    private void InitializeSpawnSettings(BranchSpawnSettingsConfig[] spawnSettingsConfig)
    {
        _spawnSettings = new BranchSpawnSettingsConfig[spawnSettingsConfig.Length];
        for (int i = 0; i < _spawnSettings.Length; i++)
        {
            _spawnSettings[i] = UnityEngine.Object.Instantiate(spawnSettingsConfig[i]);
            _spawnSettings[i].BranchCount = _growSettings.BranchesCount;
        }
    }

    private void InitializeBranchFactoryControllers()
    {
        var branchContainer = new GameObject("Branches");
        branchContainer.transform.SetParent(_tree.transform);
        _branchFactoryControllers = new BranchesFactoryController[_spawnSettings.Length];
        for (int i = 0; i < _branchFactoryControllers.Length; i++)
        {
            var factory = new BranchFactory(i + 1);
            factory.LoadResources();
            _branchFactoryControllers[i] = new BranchesFactoryController(factory, branchContainer, _spawnSettings[i]);
        }
    }

    public void SpawnBranches(List<BranchSaveConfig> branchSaveConfig, Transform relativeObj, int branchLevel)
    {
        if (branchLevel > _growSettings.BranchingValue) return;

        var newBranches = new List<Branch>();
        var nextLevel = branchLevel + 1;
        foreach (var config in branchSaveConfig)
        {
            if (config.BranchLevel == branchLevel)
            {
                if (relativeObj.TryGetComponent<IGrowable>(out var relativeGrowable))
                {


                    if (relativeGrowable.GetIndex() != config.RelativeIndex || relativeGrowable.GetGrowableTransform() == null) continue;
                }

                var branchFactoryController = _branchFactoryControllers[config.BranchLevel - 1];
                var spawnPos = relativeObj.InverseTransformPoint(config.Position);
                spawnPos = new Vector2(spawnPos.x, spawnPos.y / relativeObj.localScale.y);

                var relativeRotation = relativeObj.position.x < 0 ? Quaternion.Inverse(relativeObj.rotation) : relativeObj.rotation;
                var rotation = Quaternion.Euler(0, 0, config.Rotation) * relativeRotation;

                var branch = branchFactoryController.SpawnByFactory(new SpawnBranchSettings(spawnPos, rotation.eulerAngles.z, config.IsRight), relativeObj) as Branch;
                branch.Index = config.Index;
                newBranches.Add(branch);
                branch.InitializeBranch(relativeObj, config.BranchLevel, config.FillValue);
                SpawnBranches(branchSaveConfig, branch.transform, nextLevel);
            }
        }
        SpawnNewBranches?.Invoke(branchLevel, newBranches.ToArray());
    }

    private void SpawnBranches(Transform relativeObj, int branchLevel)
    {
        if (branchLevel > _growSettings.BranchingValue) return;

        var newBranches = _branchFactoryControllers[branchLevel - 1].SpawnByFactoryWithRandomSettings(relativeObj) as Branch[];
        if (branchLevel == 2) Debug.Log(newBranches.Length);
        var nextBranchLevel = branchLevel + 1;
        foreach (var branch in newBranches)
        {

            branch.InitializeBranch(relativeObj, branchLevel, 0);
            SpawnBranches(branch.transform, nextBranchLevel);
        }
        SpawnNewBranches?.Invoke(branchLevel, newBranches);
    }

    private void SpawnCoins(Transform relativeObj)
    {
        _coinsFactoryController.SpawnCoins(relativeObj);
    }
}
