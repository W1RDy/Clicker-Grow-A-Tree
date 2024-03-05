using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowController : IService
{ 
    private GrowSettings _growSettings;
    private ScoreCounter _scoreCounter;
    private GrowablesService _growablesService;
    private Tree _tree;

    public GrowController(GrowSettings growSettings) 
    {
        _growSettings = growSettings;
        _scoreCounter = ServiceLocator.Instance.Get<ScoreCounter>();
        _growablesService = ServiceLocator.Instance.Get<GrowablesService>();
        _tree = _growablesService.GetTree();
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
        foreach (Branch branch in _growablesService.GetBranches())
        {
            branch.Grow(_growSettings.BranchesGrowSpeed * branch.Height);
        }
    }
}
