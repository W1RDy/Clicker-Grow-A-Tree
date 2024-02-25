using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowController : IService
{ 
    private GrowSettings _growSettings;
    private Tree _tree;
    private ScoreCounter _scoreCounter;
    private GameController _gameController;

    private List<Branch> _branches = new List<Branch>();
    private Action<Branch[]> AddNewBranches;
    
    public GrowController(GrowSettings growSettings) 
    {
        _growSettings = growSettings;
        _tree = ServiceLocator.Instance.Get<Tree>();
        _scoreCounter = ServiceLocator.Instance.Get<ScoreCounter>();
        _gameController = ServiceLocator.Instance.Get<GameController>();

        AddNewBranches = branches =>
        {
            foreach (var branch in branches)
            {
                _branches.Add(branch);
            }
        };
        _tree.SpawnNewBranches += AddNewBranches;
        _gameController.FinishGame += OnFinishGame;

        _tree.InitializeTree(_growSettings);
    }

    public void Grow()
    {
        GrowTrunk();
        GrowBranches();
    }

    public void GrowTrunk()
    {
        _tree.Grow(_growSettings.TrunkGrowSpeed);
        _scoreCounter.UpdateScore(_tree.GetHeight());
    }

    private void GrowBranches()
    {
        var branches = new List<Branch>(_branches);
        foreach (Branch branch in branches)
        {
            if (branch == null) _branches.Remove(branch);
            else branch.Grow(_growSettings.BranchesGrowSpeed * branch.Height);
        }
    }

    public void OnFinishGame()
    {
        Debug.Log("Finish");
        _tree.SpawnNewBranches -= AddNewBranches;
        _gameController.FinishGame -= OnFinishGame;
    }
}
