using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsCounter : IService
{
    public int Coins { get; private set; }
    private CoinsIndicator _indicator;

    public CoinsCounter(CoinsIndicator indicator)
    {
        _indicator = indicator;
    }

    public void AddCoins(int coins)
    {
        Coins += coins;
        _indicator.SetCoins(Coins);
    }

    public void RemoveCoins(int coins)
    {
        Coins -= coins;
        if (Coins < 0) Coins = 0;
        _indicator.SetCoins(Coins);
    }
}
