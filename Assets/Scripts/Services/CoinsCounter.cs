using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsCounter : IService
{
    public int Coins { get; private set; }
    private CoinsIndicator _indicator;
    private SaveService _saveService;
    private Action SaveData;
    private Action Unsubscribe;

    public CoinsCounter(CoinsIndicator indicator)
    {
        _indicator = indicator;
        _saveService = ServiceLocator.Instance.Get<SaveService>();
        Coins = _saveService.DataContainer.Coins;
        _indicator.SetCoins(Coins);

        SaveData = () =>
        {
            _saveService.SaveCoins(Coins);
        };

        Unsubscribe = () =>
        {
            _saveService.SaveDataOnQuit -= SaveData;
            _saveService.QuitApplication -= Unsubscribe;
        };

        _saveService.QuitApplication += Unsubscribe;

        _saveService.SaveDataOnQuit += SaveData;
    }

    public void AddCoins(int coins)
    {
        Coins += coins;
        _indicator.SetCoins(Coins);
        _saveService.SaveAllData();
    }

    public void RemoveCoins(int coins)
    {
        Coins -= coins;
        if (Coins < 0) Coins = 0;
        _indicator.SetCoins(Coins);
    }
}
