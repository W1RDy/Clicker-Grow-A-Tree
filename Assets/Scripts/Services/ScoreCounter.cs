using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : IService
{
    private int _score;
    private ScoreIndicator _scoreIndicator;
    private SaveService _saveService;
    private Action SaveData;
    private Action Unsubscribe;

    public void InitializeCounter()
    {
        _scoreIndicator = ServiceLocator.Instance.Get<ScoreIndicator>();
        _saveService = ServiceLocator.Instance.Get<SaveService>();

        _score = _saveService.DataContainer.Score;
        _scoreIndicator.SetScore(_score);

        SaveData = () =>
        {
            _saveService.SaveScore(_score);
        };

        Unsubscribe = () =>
        {
            _saveService.SaveDataOnQuit -= SaveData;
            _saveService.QuitApplication -= Unsubscribe;
        };

        _saveService.QuitApplication += Unsubscribe;

        _saveService.SaveDataOnQuit += SaveData;
    }

    public void UpdateScore(float value)
    {
        _score = (int)Mathf.Ceil(value * 10);
        InteractorWithBrowser.SaveScoreInLeaderboard(_score);
        _scoreIndicator.SetScore(_score);
    }
}
