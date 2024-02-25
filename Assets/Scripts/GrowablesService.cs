using System;
using System.Collections.Generic;
using UnityEngine;

public class GrowablesService : IService
{
    private Tree _tree;
    private Action<int, Branch[]> AddNewBranches;
    private Action<int, Branch> RemoveOddBranches;
    private GameController _gameController;

    private Dictionary<int, List<Branch>> _branches = new Dictionary<int, List<Branch>>();

    public GrowablesService()
    {
        _tree = ServiceLocator.Instance.Get<Tree>();
        _gameController = ServiceLocator.Instance.Get<GameController>();
        _gameController.FinishGame += OnFinishGame;

        RemoveOddBranches = (branchLevel, branch) =>
        {
            branch.Destory -= RemoveOddBranches;
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
                branchesList.Add(branch);
            }
        };

        _tree.SpawnNewBranches += AddNewBranches;
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

    public void OnFinishGame()
    {
        Debug.Log("Finish");
        _tree.SpawnNewBranches -= AddNewBranches;
        _gameController.FinishGame -= OnFinishGame;
    }

    public int GetBranchingValue() => _branches.Count;
}


