using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : IService
{
    private int _score;
    private ScoreIndicator _scoreIndicator;

    public void InitializeCounter()
    {
        _scoreIndicator = ServiceLocator.Instance.Get<ScoreIndicator>();
    }

    public void UpdateScore(float value)
    {
        _score = (int)Mathf.Ceil(value * 10);
        _scoreIndicator.SetScore(_score);
    }
}
