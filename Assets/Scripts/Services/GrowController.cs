using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowController : IService
{
    private float _growValue = 0.02f;
    private Tree _tree;
    private ScoreCounter _scoreCounter;
    
    public GrowController() 
    {
        _tree = ServiceLocator.Instance.Get<Tree>();
        _scoreCounter = ServiceLocator.Instance.Get<ScoreCounter>();
    }

    public void Grow()
    {
        _tree.Grow(_growValue);
        _scoreCounter.UpdateScore(_tree.GetHeight());
    }
}
