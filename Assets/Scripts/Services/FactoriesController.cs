using System;
using UnityEngine;

public class FactoriesController : IService
{
    private CoinsFactoryController _coinsFactoryController;
    private BranchesFactoryController[] _branchFactoryControllers;

    private BranchSpawnSettingsConfig[] _spawnSettings;
    private Action<Transform> _trunkSpawnCallback;
    public event Action<int, Branch[]> SpawnNewBranches;

    private Tree _tree;

    public void InitializeController(BranchSpawnSettingsConfig[] spawnSettingsConfig, CoinsSpawnSettings coinsSpawnSettings)
    {
        _tree = ServiceLocator.Instance.Get<Tree>();
        InitializeSpawnSettings(spawnSettingsConfig);
        InitializeBranchFactoryControllers();
        _coinsFactoryController = new CoinsFactoryController(coinsSpawnSettings);

        _trunkSpawnCallback = relativeObj =>
        {
            foreach (var spawnSetting in _spawnSettings)
            {
                spawnSetting.BranchCount = _tree.GrowSettings.BranchesCount;
            }

            SpawnBranches(relativeObj, 1);
            SpawnCoins();
        };
        _tree.InitializeTree(_trunkSpawnCallback);
    }

    private void InitializeSpawnSettings(BranchSpawnSettingsConfig[] spawnSettingsConfig)
    {
        _spawnSettings = new BranchSpawnSettingsConfig[spawnSettingsConfig.Length];
        for (int i = 0; i < _spawnSettings.Length; i++)
        {
            _spawnSettings[i] = UnityEngine.Object.Instantiate(spawnSettingsConfig[i]);
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

    private void SpawnBranches(Transform relativeObj, int branchLevel)
    {
        if (branchLevel > _tree.GrowSettings.BranchingValue) return;

        var newBranches = _branchFactoryControllers[branchLevel - 1].SpawnByFactoryWithRandomSettings(relativeObj) as Branch[];
        var nextBranchLevel = branchLevel + 1;
        foreach (var branch in newBranches)
        {
            branch.InitializeBranch(relativeObj, branchLevel);
            SpawnBranches(branch.transform, nextBranchLevel);
        }
        SpawnNewBranches?.Invoke(branchLevel, newBranches);
    }

    private void SpawnCoins()
    {
        _coinsFactoryController.SpawnCoins(5);
    }
}
