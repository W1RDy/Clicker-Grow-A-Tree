using System;
using System.Collections.Generic;
using UnityEngine;

public class GrowablesService : IService
{
    private FactoriesController _factoriesController;
    private Tree _tree;
    private Action<int, Branch[]> AddNewBranches;
    private Action<int, Branch> RemoveOddBranches;
    private GameController _gameController;
    private IndexDistributor _indexDistributor;

    private Dictionary<int, List<Branch>> _branches = new Dictionary<int, List<Branch>>();

    private SaveService _saveService;

    private Action SaveData;

    public GrowablesService()
    {
        _tree = ServiceLocator.Instance.Get<Tree>();
        _gameController = ServiceLocator.Instance.Get<GameController>();
        _gameController.FinishGame += OnFinishGame;
        _saveService = ServiceLocator.Instance.Get<SaveService>();

        _indexDistributor = new IndexDistributor();

        RemoveOddBranches = (branchLevel, branch) =>
        {
            branch.Destory -= RemoveOddBranches;
            _indexDistributor.AddFreeIndex(branch.Index);
            _branches[branchLevel].Remove(branch);
        };

        AddNewBranches = (branchLevel, branches) =>
        {
            List<Branch> branchesList;
            if (!_branches.TryGetValue(branchLevel, out branchesList))
            {
                _branches[branchLevel] = branchesList = new List<Branch>();
            }

            foreach (var branch in branches)
            {
                branch.Destory += RemoveOddBranches;
                branch.Index = _indexDistributor.GetFreeIndex();
                branchesList.Add(branch);
            }
        };

        SaveData = () =>
        {
            _saveService.SaveBranchesInContainer(GetBranches());
            _saveService.SaveDataOnQuit -= SaveData;
        };

        _saveService.SaveDataOnQuit += SaveData;
    }

    public void InitializeService(FactoriesController factoriesController)
    {
        _factoriesController = factoriesController;
        _factoriesController.SpawnNewBranches += AddNewBranches;
    }

    public Tree GetTree()
    {
        return _tree;
    }

    public List<Branch> GetBranches()
    {
        List<Branch> lists = new List<Branch>();
        foreach (var branches in _branches.Values)
        {
            lists.AddRange(branches);
        }
        return lists;
    }

    public List<Branch> GetBranches(int level)
    {
        return _branches[level];
    }

    public List<Branch> GetBranches(int level, Transform relativeObj)
    {
        var branches = GetBranches(level);
        List<Branch> result = new List<Branch>();
        foreach(var branch in branches)
        {
            IGrowable growable = branch;
            for (int i = 0; i < level; i++)
            {
                growable = growable.GetRelativeGrowable();

                if (growable == null) break;
                if (relativeObj == growable.GetGrowableTransform())
                {
                    Debug.Log(branch);
                    result.Add(branch);
                    break;
                }
            }
        }
        return result;
    }

    public void OnFinishGame()
    {
        Debug.Log("Finish");
        _factoriesController.SpawnNewBranches -= AddNewBranches;
        _gameController.FinishGame -= OnFinishGame;
    }

    public int GetBranchingValue() => _branches.Count;
}
